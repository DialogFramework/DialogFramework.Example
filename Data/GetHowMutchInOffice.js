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
    text += 'В комнате номер ' + office + ' работает ' + offices[i].Entities.ToList().Count;
}
messages.Add(text);
