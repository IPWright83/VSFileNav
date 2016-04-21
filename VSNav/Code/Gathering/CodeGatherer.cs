using EnvDTE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VSNav
{
    /// <summary>
    /// Gather classes on top of files
    /// </summary>
    public class ClassGatherer : Gatherer
    {
        // List of all the found classes
        private List<CodeClass> classes = new List<CodeClass>();

        /// <summary>
        /// Initializes a new instance of the <see cref="ClassGatherer"/> class.
        /// </summary>
        /// <param name="manager">The manager.</param>
        public ClassGatherer(EventManager manager)
            : base(manager)
        { }

        #region Methods

        /// <inheritdoc />
        public override void Process(ProjectItem item, ProjectInfo projectInfo)
        {
            List<CodeClass> elements = GetCodeElements(item);
            classes.AddRange(elements);

            //       inProjectItem.Open.Activate()
            //Dim startPoint As TextPoint = classMember.StartPoint
            //Dim sel As TextSelection = dte.ActiveDocument.Selection
            //sel.GotoLine(startPoint.Line, True)
        }

        /// <summary>
        /// Always gather on a background thread due to the processing time
        /// </summary>
        public override void Gather()
        {
            base.GatherBackground();
        }

        /// <inheritdoc/>
        public override void Clear()
        {
            this.classes.Clear();
        }

        /// <summary>
        /// Grab the set of CodeElements (classes) for a Project item
        /// </summary>
        /// <param name="item">The item to look through</param>
        /// <returns>The set of Classes</returns>
        private List<CodeClass> GetCodeElements(ProjectItem item)
        {
            List<CodeClass> retVal = new List<CodeClass>();

            // Grab the code Model
            FileCodeModel codeModel = item.FileCodeModel;
            if (codeModel != null)
            {
                // Grab the code elements
                CodeElements elements = codeModel.CodeElements;
                for (int j = 1; j <= elements.Count; ++j)
                {
                    CodeElement element = elements.Item(j);
                    if (element.Kind == vsCMElement.vsCMElementNamespace)
                    {
                        CodeNamespace cns = (CodeNamespace)element;
                        CodeElements melements = cns.Members;

                        // Grab all the classes
                        for (int k = 1; k <= melements.Count; ++k)
                        {
                            CodeElement melemt = melements.Item(k);
                            if (melemt.Kind == vsCMElement.vsCMElementClass)
                            {
                                if (!melemt.Name.EndsWith("Resources") && !melemt.Name.EndsWith("Settings"))
                                {
                                    retVal.Add((CodeClass)melemt);
                                }
                            }
                        }
                    }
                }
            }

            return retVal;
        }

        /// <inheritdoc/>
        public override void Remove(Project project)
        {
            base.Remove(project);
        }

        /// <inheritdoc/>
        public override void Remove(ProjectItem item)
        {
            base.Remove(item);
        }

        /// <inheritdoc/>
        public override void Rename(Project project, string oldName)
        {
            base.Rename(project, oldName);
        }

        /// <inheritdoc/>
        public override void Rename(ProjectItem item, string oldName)
        {
            base.Rename(item, oldName);
        }

        #endregion
    }
}
