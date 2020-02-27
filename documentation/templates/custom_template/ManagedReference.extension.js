exports.preTransform = function (model) {
  transformItem(model, 1);

  return model;

  function transformItem(item, level) {

      var log = "====================================================\n";

      var isAssembly = item.parent == null && item.assemblies.indexOf(item.uid) !== -1;

      var shared = item.__global._shared[item._tocKey];
      var items = shared.items;
      var temp_items = [];

      log += JSON.stringify(item.assemblies) + "\n";

      for (var i = 0; i < item.assemblies.length; i++)
      {
          var assemblyName = item.assemblies[i];
          if (item.uid.indexOf(assemblyName) !== -1)
          {
              temp_items.push({
                  "name": assemblyName,
                  "href": assemblyName + ".html",
                  "topicHref": assemblyName + ".html",
                  "topicUid": assemblyName,
                  "items": items
              });
          } else {
              temp_items = items;
          }
      }

      shared.items = temp_items;


      log += JSON.stringify(model) + "\n";
      log += "====================================================";

      console.log(log);
  }
}