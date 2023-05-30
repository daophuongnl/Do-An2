using Autodesk.Revit.DB;
using SingleData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Entity
{
    public static class ShareParameterFactoryUtil
    {
        private static RevitData revitData => RevitData.Instance;

        public static DefinitionGroup GetDefinitionGroup(this ShareParameterFactory q)
        {
            var definitionFile = q.Config.DefinitionFile;
            var definitionGroups = definitionFile.Groups;

            var definitionGroupName = q.DefinitionGroupName;

            var definitionGroup = definitionGroups.FirstOrDefault(x => x.Name == definitionGroupName);
            if (definitionGroup == null)
            {
                definitionGroup = definitionGroups.Create(definitionGroupName);
            }

            return definitionGroup;
        }

        public static List<Category> GetCategories(this ShareParameterFactory q)
        {
            var builtInCategories = q.BuiltInCategories!;
            var allCategories = revitData.Categories;

            return allCategories.Where(x => builtInCategories.Contains((BuiltInCategory)x.Id.IntegerValue)).ToList();
        }

        public static CategorySet GetCategorySet(this ShareParameterFactory q)
        {
            var categorySet = new CategorySet();

            q.Categories.ForEach(x => categorySet.Insert(x));

            return categorySet;
        }

        public static Definition GetDefinition(this ShareParameterFactory q)
        {
            var definitions = q.DefinitionGroup.Definitions;
            var name = q.Name;

            var definition = definitions.FirstOrDefault(x => x.Name == name);
            if (definition == null)
            {
#if REVIT2022_OR_LESS
                var parameterType = q.ParameterType;
                var options = new ExternalDefinitionCreationOptions(name, parameterType);
#else
                var forgeTypeId = q.ForgeTypeId;
                var options = new ExternalDefinitionCreationOptions(name, forgeTypeId);
#endif
                definition = definitions.Create(options);
            }
            else
            {
                q.IsNameExisted = true;
            }

            return definition;
        }

        public static Binding GetBinding(this ShareParameterFactory q)
        {
            var doc = revitData.Document;

            var categories = q.Categories;
            var definition = q.Definition;
            var parameterGroup = q.ParameterGroup;

            InstanceBinding? binding = null;

            if (q.IsNameExisted)
            {
                binding = (doc.ParameterBindings.get_Item(definition) as InstanceBinding)!;
                if (binding != null)
                {
                    var categorySet = binding.Categories;
                    categories.ForEach(category =>
                    {
                        if (!categorySet.Contains(category))
                        {
                            categorySet.Insert(category);
                        }
                    });
                }
            }

            if (binding == null)
            {
                var categorySet = new CategorySet();
                categories.ForEach(x => categorySet.Insert(x));

                binding = new InstanceBinding
                {
                    Categories = categorySet
                };
                doc.ParameterBindings.Insert(definition, binding, parameterGroup);
            }
            else
            {
                doc.ParameterBindings.ReInsert(definition, binding, parameterGroup);
            }

            return binding!;
        }

        //
        public static void Do(this ShareParameterFactory q)
        {
            var binding = q.Binding;
        }
    }
}
