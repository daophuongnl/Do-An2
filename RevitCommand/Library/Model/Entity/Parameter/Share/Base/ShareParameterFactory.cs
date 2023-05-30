using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.Entity.ShareParameterFactoryNS;

namespace Model.Entity
{
    public class ShareParameterFactory
    {
        private Config? config;
        public Config Config
        {
            get => this.config ??= new Config();
            set => this.config = value;
        }

        private string? definitionGroupName;
        public string? DefinitionGroupName
        {
            get => this.definitionGroupName ??= this.Config.DefinitionGroupName;
            set => this.definitionGroupName = value; 
        }

        private string? name;
        public string? Name
        {
            get => this.name ??= this.Config.Name;
            set => this.name = value;
        }


#if REVIT2022_OR_LESS
        private ParameterType? parameterType;
        public ParameterType ParameterType
        {
            get => this.parameterType ??= this.Config.ParameterType;
            set => this.parameterType = value;
        }
#else
        private ForgeTypeId? forgeTypeId;
        public ForgeTypeId ForgeTypeId
        {
            get => this.forgeTypeId ??= this.Config.ForgeTypeId;
            set => this.forgeTypeId = value;
        }
#endif

        private BuiltInParameterGroup? parameterGroup;
        public BuiltInParameterGroup ParameterGroup
        {
            get => this.parameterGroup ??= this.Config.ParameterGroup;
            set => this.parameterGroup = value;
        }

        private DefinitionGroup? definitionGroup;
        public DefinitionGroup DefinitionGroup => this.definitionGroup ??= this.GetDefinitionGroup();

        private List<BuiltInCategory>? builtInCategories;
        public List<BuiltInCategory>? BuiltInCategories
        {
            get => this.builtInCategories ??= this.Config.BuiltInCategories;
            set => this.builtInCategories = value;
        }

        private List<Category>? categories;
        public List<Category> Categories => this.categories ??= this.GetCategories();

        private Definition? definition;
        public Definition Definition => this.definition ??= this.GetDefinition();

        private Binding? binding;
        public Binding Binding => this.binding ??= this.GetBinding();

        public bool IsNameExisted { get; set; } = false;
    }
}
