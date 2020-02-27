exports.preTransform = function (model) {
  transformItem(model, 1);

  return model;

  function transformItem(item, level) {

      var log = "====================================================\n";

      var isAssembly = item.parent == null && item.assemblies.indexOf(item.uid) !== -1;

      var shared = item.__global._shared[item._tocKey];
      var items = shared.items;
      var temp_items = [];

      var rootNamespaces = getAllRootNamespaces(items);
      var assemblies = getCommonParts(rootNamespaces);

      temp_items = getItemsForAssemblies(assemblies);

      log += JSON.stringify(temp_items) + "\n";

      //log += JSON.stringify(item.assemblies) + "\n";


      for (var i = 0; i < temp_items.length; i++)
      {
        // TODO YP: aggregate items by assemblies
      }

      /*
      for (var i = 0; i < item.assemblies.length; i++)
      {
          var assemblyName = item.assemblies[i];
          var asmblIndex = findAssemblyWithName(temp_items, assemblyName);

          if (asmblIndex === -1)
          {
              console.error("FATAL TOC ERROR !!!");
          }
          else
          {
              temp_items[asmblIndex].items.push()
          }


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
      */

      shared.items = temp_items;


      log += JSON.stringify(model) + "\n";
      log += "====================================================";

      console.log(log);
  }

  function findAssemblyWithName(items, name)
  {
      for (var i = 0; i < items.length; i++)
      {
          if (items[i].name === name)
          {
              return i;
          }
      }
      return -1;
  }

  function getItemsForAssemblies(assemblies)
  {
      var tmp = [];
      for (var i = 0; i < assemblies.length; i++)
      {
          var assemblyName = assemblies[i];
          tmp.push({
              "name": assemblyName,
              "href": assemblyName + ".html",
              "topicHref": assemblyName + ".html",
              "topicUid": assemblyName,
              "items": []
          });
      }
      return tmp;
  }

  function getAllRootNamespaces(items)
  {
      var tmp = [];
      for (var i = 0; i < items.length; i++)
      {
          tmp.push(items[i].name);
      }
      return tmp;
  }

    function getCommonParts(namespaces)
    {
        var commonParts = [];
        var xtoolkit = "Softeq.XToolkit.";

        for (var i = 0; i < namespaces.length; i++)
        {
            var cuttedName = namespaces[i].replace(xtoolkit, "");
            var nameParts = cuttedName.split(".");

            if (nameParts.length === 1)
            {
                commonParts.push(namespaces[i]);
            }
            else
            {
                aggregateForPlatform("iOS");
                aggregateForPlatform("Droid");
            }
        }

        function aggregateForPlatform(platform)
        {
            var platformIndex = nameParts.indexOf(platform);
            if (platformIndex === -1)
            {
                return;
            }
            var platformNamespace = xtoolkit + nameParts[platformIndex - 1] + "." + platform;
            if (commonParts.indexOf(platformNamespace) === -1)
            {
                commonParts.push(platformNamespace);
            }
        }

        return commonParts;
    }
}