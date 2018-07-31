var tms = eventArgs["tms"];
var intent = eventArgs["intent"];
var persons = EntityQuery.Entities(tms).Class("Person").ToList();
var text = "";
var first = true;
for (var i = 0; i < persons.Count; i++) {
    var person = persons[i];
    if (!first) {
        text += '\n';
    }
    first = false;
    text += person["fio"] + ' - ' + person["email"];
}
messages.Add(text);
