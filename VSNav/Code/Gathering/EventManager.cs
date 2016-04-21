using EnvDTE;
using EnvDTE80;
using Microsoft.VisualStudio.Shell;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VSNav
{
    /// <summary>
    /// Used to hook up to Environment Events so Gathers can decide when to update.
    /// </summary>
    /// <remarks>Essentially this just hooks up to events and passes them through</remarks>
    public class EventManager
    {
        #region Locals

        // Keep a handle to the SolutionEvents - http://stackoverflow.com/questions/7825489/how-do-i-subscribe-to-solution-and-project-events-from-a-vspackage
        private SolutionEvents solutionEvents;

        // Keep handles to ProjectEvents
        ProjectItemsEvents projectEvents;
        ProjectItemsEvents cSharpProjectEvents;
        ProjectItemsEvents vbProjectEvents;
        ProjectItemsEvents webProjectEvents;

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="EventManager"/> class.
        /// </summary>
        public EventManager()
        {
            this.dte = GetDTE();
        }

        #region Properties

        /// <summary>
        /// Gets the DTE / Environment.
        /// </summary>
        /// <value>The DTE.</value>
        public DTE DTE
        {
            get
            {
                if (this.dte == null)
                    this.dte = GetDTE();

                return this.dte;
            }
        }
        private DTE dte;

        /// <summary>
        /// Solution Opened event
        /// </summary>
        public event _dispSolutionEvents_OpenedEventHandler SolutionOpened;

        /// <summary>
        /// Project Added Event
        /// </summary>
        public event _dispSolutionEvents_ProjectAddedEventHandler ProjectAdded;
        
        /// <summary>
        /// Project Removed Event
        /// </summary>
        public event _dispSolutionEvents_ProjectRemovedEventHandler ProjectRemoved;

        /// <summary>
        /// Project Renamed Event
        /// </summary>
        public event _dispSolutionEvents_ProjectRenamedEventHandler ProjectRenamed;

        /// <summary>
        /// Item is Added to a Project
        /// </summary>
        public event _dispProjectItemsEvents_ItemAddedEventHandler ProjectItemAdded;

        /// <summary>
        /// Item is Removed from a Project
        /// </summary>
        public event _dispProjectItemsEvents_ItemRemovedEventHandler ProjectItemRemoved;

        /// <summary>
        /// Item is Renamed in a Project
        /// </summary>
        public event _dispProjectItemsEvents_ItemRenamedEventHandler ProjectItemRenamed;

        #endregion

        #region Methods

        /// <summary>
        /// Called when the Solution is opened and fires an Event
        /// </summary>
        public virtual void OnSolutionOpened()
        {
            Debug.WriteLine(string.Format(CultureInfo.CurrentCulture, "OnSolutionOpened fired for: {0}", this.ToString()));
            _dispSolutionEvents_OpenedEventHandler solutionOpenedHandler = this.SolutionOpened;
            if (solutionOpenedHandler != null)
            {
                solutionOpenedHandler();
            }
        }

        /// <summary>
        /// Called when a Project is added to the Solution.
        /// </summary>
        /// <param name="project">The project.</param>
        public virtual void OnProjectAdded(Project project)
        {
            Debug.WriteLine(string.Format(CultureInfo.CurrentCulture, "OnProjectAdded fired for: {0}", this.ToString()));
            _dispSolutionEvents_ProjectAddedEventHandler projectAddedHandler = this.ProjectAdded;
            if (projectAddedHandler != null)
            {
                projectAddedHandler(project);
            }
        }

        /// <summary>
        /// Called when a Project is removed from the Solution.
        /// </summary>
        /// <param name="project">The project.</param>
        public virtual void OnProjectRemoved(Project project)
        {
            Debug.WriteLine(string.Format(CultureInfo.CurrentCulture, "OnProjectRemoved fired for: {0}", this.ToString()));
            _dispSolutionEvents_ProjectRemovedEventHandler projectRemovedHandler = this.ProjectRemoved;
            if (projectRemovedHandler != null)
            {
                projectRemovedHandler(project);
            }
        }

        /// <summary>
        /// Called when a Project is renamed in the Solution.
        /// </summary>
        /// <param name="project">The project.</param>
        public virtual void OnProjectRenamed(Project project, String oldName)
        {
            Debug.WriteLine(string.Format(CultureInfo.CurrentCulture, "OnProjectRenamed fired for: {0}", this.ToString()));
            _dispSolutionEvents_ProjectRenamedEventHandler projectRenamedHandler = this.ProjectRenamed;
            if (projectRenamedHandler != null)
            {
                projectRenamedHandler(project, oldName);
            }
        }

        /// <summary>
        /// Called when project item is added.
        /// </summary>
        /// <param name="item">The item.</param>
        public virtual void OnProjectItemAdded(ProjectItem item)
        {
            Debug.WriteLine(string.Format(CultureInfo.CurrentCulture, "OnProjectItemAdded fired for: {0}", this.ToString()));
            _dispProjectItemsEvents_ItemAddedEventHandler projectitemAddedHandler = this.ProjectItemAdded;
            if (projectitemAddedHandler != null)
            {
                projectitemAddedHandler(item);
            }
        }

        /// <summary>
        /// Called when a project item is removed.
        /// </summary>
        /// <param name="item">The item.</param>
        public virtual void OnProjectItemRemoved(ProjectItem item)
        {
            Debug.WriteLine(string.Format(CultureInfo.CurrentCulture, "OnProjectItemRemoved fired for: {0}", this.ToString()));
            _dispProjectItemsEvents_ItemRemovedEventHandler projectitemRemovedHandler = this.ProjectItemRemoved;
            if (projectitemRemovedHandler != null)
            {
                projectitemRemovedHandler(item);
            }
        }

        /// <summary>
        /// Called when project item is renamed.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <param name="oldName">The old name.</param>
        public virtual void OnProjectItemRenamed(ProjectItem item, String oldName)
        {
            Debug.WriteLine(string.Format(CultureInfo.CurrentCulture, "OnProjectItemRenamed fired for: {0}", this.ToString()));
            _dispProjectItemsEvents_ItemRenamedEventHandler projectitemRenamedHandler = this.ProjectItemRenamed;
            if (projectitemRenamedHandler != null)
            {
                projectitemRenamedHandler(item, oldName);
            }
        }

        /// <summary>
        /// Wire up the Project Events
        /// </summary>
        private void HookupProjectItemsEventListeners(ProjectItemsEvents projectItemsEvents)
        {
            if (projectItemsEvents == null)
                return;

            projectItemsEvents.ItemAdded += (p) => this.OnProjectItemAdded(p);
            projectItemsEvents.ItemRemoved += (p) => this.OnProjectItemRemoved(p);
            projectItemsEvents.ItemRenamed += (p, name) => this.OnProjectItemRenamed(p, name);
        }

        /// <summary>
        /// Attempts to retrieve the DTE;
        /// </summary>
        private DTE GetDTE()
        {
            DTE dte = Package.GetGlobalService(typeof(DTE)) as DTE;

            // If events haven't yet been wired up.
            if (dte != null)
            {
                this.solutionEvents = dte.Events.SolutionEvents;
                this.solutionEvents.Opened += () => this.OnSolutionOpened();
                this.solutionEvents.ProjectAdded += (p) => this.OnProjectAdded(p);
                this.solutionEvents.ProjectRemoved += (p) => this.OnProjectRemoved(p);
                this.solutionEvents.ProjectRenamed += (p, name) => this.OnProjectRenamed(p, name);

                // Obtain all the project events
                Events2 events2 = dte.Events as Events2;
                if (events2 != null)
                {
                    this.projectEvents = events2.ProjectItemsEvents;
                    HookupProjectItemsEventListeners(this.projectEvents);
                }
                this.cSharpProjectEvents = dte.Events.GetObject("CSharpProjectItemsEvents") as ProjectItemsEvents;
                this.vbProjectEvents = dte.Events.GetObject("VBProjectItemsEvents") as ProjectItemsEvents;
                this.webProjectEvents = dte.Events.GetObject("WebSiteItemsEvents") as ProjectItemsEvents;

                // Wire up events
                HookupProjectItemsEventListeners(this.projectEvents);
                HookupProjectItemsEventListeners(this.cSharpProjectEvents);
                HookupProjectItemsEventListeners(this.vbProjectEvents);
                HookupProjectItemsEventListeners(this.webProjectEvents);
            }

            return dte;
        }

        #endregion
    }
}
