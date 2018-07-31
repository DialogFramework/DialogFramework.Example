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
    text += 'Подразделение: ' + department["name"];

    var offices = EntityQuery.Entities("Person").Where("department", department.ID).GroupBy("office");
    for (var i = 0; i < offices.Count; i++) {
        text += '\nкомната номер ' + offices[i].Key;
    }
}
messages.Add(text);
