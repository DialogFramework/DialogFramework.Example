var tms = eventArgs["tms"];
var intent = eventArgs["intent"];
var text = "";
var phones = EntityQuery.Entities(tms).Class("Person").GroupBy("phone");
for (var pi = 0; pi < phones.Count; pi++) {
    var phone = phones[pi].Key;
    text += 'Телефон: ' + phone;
    var persons = phones[pi].Entities.Distinct().ToList();
    for (var i = 0; i < persons.Count; i++) {
        var person = persons[i];
        text += '\n' + person["fio"];
    }
}
messages.Add(text);
