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
    var count = EntityQuery.Entities("Person").Where("department", department.ID).ToList().Count;
    text += 'В отделе ' + department["name"] + ' работает ' + count;
}
messages.Add(text);
