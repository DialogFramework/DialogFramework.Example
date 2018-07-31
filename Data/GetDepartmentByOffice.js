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
    text += 'В комнате ' + office + ' находится';
    var departments = offices[i].Entities.GroupBy("department");
    for (var di = 0; di < departments.Count; di++) {
        var department = EntityQuery.GetEntity("Department", departments[di].Key)["name"];
        text += '\n' + department;
    }
}
messages.Add(text);

