var tms = eventArgs["tms"];
var intent = eventArgs["intent"];
var departments = EntityQuery.Entities(tms).Class("Department").ToList();
var text = "";
var first = true;
for (var di = 0; di < departments.Count; di++) {
    var department = departments[di];
    if (!first) {
        text += '\n';
    }
    first = false;

    text += department["name"];

    var persons = EntityQuery.Entities("Person").Where("department", department.ID).ToList();
    for (var i = 0; i < persons.Count; i++) {
        var person = persons[i];
        text += '\n' + person["fio"];
    }
}
messages.Add(text);