var tms = eventArgs["tms"];
var intent = eventArgs["intent"];
var offices = EntityQuery.Entities(tms).Class("Person").GroupBy("office");
var text = "";
var first = true;
for (var i = 0; i < offices.Count; i++) {
    var office = offices[i].Key;
    if (!first) {
        text += '\n';
    }
    first = false;
    text += 'Комната номер ' + office + ':';
    var persons = offices[i].Entities.Distinct().ToList();
    for (var i = 0; i < persons.Count; i++) {
        var person = persons[i];
        text += '\n' + person["fio"] + ' - ' + person["phone"];
    }
}
messages.Add(text);
