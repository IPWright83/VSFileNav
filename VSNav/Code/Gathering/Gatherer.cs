using EnvDTE;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace VSNav
{
    /// <summary>
    /// Represents a Gatherer that knows how to process a solution
    /// </summary>
    public abstract class Gatherer
    {
        /// <summary>
        /// Dictionary of the last event args to prevent events firing too many times.
        /// </summary>
        private Dictionary<String, object[]> lastParams = new Dictionary<string, object[]>();

        /// <summary>
        /// Whether the thread should cancel
        /// </summary>
        protected bool cancel;

        /// <summary>
        /// Whether the gatherer is running
        /// </summary>
        protected bool isRunning;

        /// <summary>
        /// The EventManager
        /// </summary>
        protected EventManager manager;

        /// <summary>
        /// Gets or sets the last search string.
        /// </summary>
        public String LastSearchString { get; set; } 

        /// <summary>
        /// Initializes a new instance of the <see cref="Gatherer"/> class.
        /// </summary>
        /// <param name="manager">The manager.</param>
        public Gatherer(EventManager manager)
        {
            if (manager == null)
                throw new ArgumentNullException("manager");

            // Wire up update events
            this.manager = manager;
            this.manager.SolutionOpened += () => { Gather(); };
            this.manager.ProjectAdded += (p) => { if(IgnoreEvent("ProjectAdded", p)) { return; } Process(p); };
            this.manager.ProjectItemAdded += (p) => { if(IgnoreEvent("ProjectItemAdded", p)) { return; } Process(p, new ProjectInfo(p.ContainingProject));  };
            this.manager.ProjectItemRemoved += (p) => { if(IgnoreEvent("ProjectItemRemoved", p)) { return; } Remove(p); };
            this.manager.ProjectRemoved += (p) => { if (IgnoreEvent("ProjectRemoved", p)) { return; } Remove(p); };
            this.manager.ProjectRenamed += (p, s) => { if (IgnoreEvent("ProjectRenamed", p, s)) { return; } Rename(p, s); };
            this.manager.ProjectItemRenamed += (p, s) => { if (IgnoreEvent("ProjectItemRenamed", p, s)) { return; } Rename(p, s); };
        }

        /// <summary>
        /// Determine if the parameter to the event was the same as the last one - if so skip the event
        /// </summary>
        private bool IgnoreEvent(string eventName, params object[] parameters)
        {
            // Check for the existence of the item
            if (this.lastParams.ContainsKey(eventName) == false)
            {
                this.lastParams[eventName] = parameters;
                return false;
            }

            // Check each parameter
            Boolean retVal = true;
            object[] lastValues = this.lastParams[eventName];
            for (int i = 0; i < lastValues.Count(); i++)
            {
                // Something was different so parameters have changed.
                if (!lastValues[i].Equals(parameters[i]))
                {
                    retVal = false;
                    break;
                }
            }

            // Update the last parameter
            if (retVal == false)
                this.lastParams[eventName] = parameters;

            return retVal;
        }
        
        /// <summary>
        /// Returns the Environment DTE object
        /// </summary>
        public DTE DTE
        {
            get { return this.manager.DTE; }
        }

        /// <summary>
        /// Runs a gather process
        /// </summary>
        public virtual void Gather()
        {
            Stopwatch sw = Stopwatch.StartNew();
            Process();
            sw.Stop();
            Debug.WriteLine(this.GetType() + ".Gather() took : " + sw.ElapsedMilliseconds + " ms");
        }

        /// <summary>
        /// Runs a gather process on a background thread
        /// </summary>
        public virtual void GatherBackground()
        {
           ThreadPool.QueueUserWorkItem(new WaitCallback((callback) =>
           {
               Stopwatch sw = Stopwatch.StartNew();
               Process();

               sw.Stop();
               Debug.WriteLine(this.GetType() + ".GatherBackground() took : " + sw.ElapsedMilliseconds + " ms");
           }));
        }

        #region Testing

        public void TimeProcess()
        {
            Stopwatch sw = Stopwatch.StartNew();

            ProcessMainThread();
            sw.Stop();
            Debug.WriteLine("ProcessMainThread took : " + sw.ElapsedMilliseconds + " ms");

            ProcessBackgroundThreadPool();
            
            ProcessCustomThread();
        }

        public void ProcessMainThread()
        {

            Process();
        }

        public void ProcessBackgroundThreadPool()
        {
            System.Threading.ThreadPool.QueueUserWorkItem(new System.Threading.WaitCallback((o) => 
            {
                Stopwatch sw = Stopwatch.StartNew();
                Process();
                sw.Stop();
                Debug.WriteLine("ProcessBackgroundThreadPool took : " + sw.ElapsedMilliseconds + " ms");
            }));
        }
        public void ProcessCustomThread() 
        {
            System.Threading.Thread t = new System.Threading.Thread(new System.Threading.ThreadStart(() =>
            {
                
                Stopwatch sw = Stopwatch.StartNew();
                Process();
                sw.Stop();
                Debug.WriteLine("ProcessCustomThread took : " + sw.ElapsedMilliseconds + " ms");
            }));
            t.ApartmentState = System.Threading.ApartmentState.MTA;
            t.Start();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Clears the results from the gatherer.
        /// </summary>
        public abstract void Clear();

        /// <summary>
        /// Processes the specified item.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <param name="projectName">Name of the project.</param>
        public abstract void Process(ProjectItem item, ProjectInfo projectInfo);

        /// <summary>
        /// Processes the specified DTE service.
        /// </summary>
        public virtual void Process()
        {
            try
            {
                this.isRunning = true;
                this.Clear();

                // Check the cancel flag
                if (this.cancel)
                    return;

                if (this.manager != null && this.manager.DTE != null)
                {
                    Process(this.manager.DTE.Solution);
                }
            }
            finally
            {
                this.isRunning = true;
            }
        }

        /// <summary>
        /// Processes the specified solution.
        /// </summary>
        public virtual void Process(Solution solution)
        {
            if (solution != null && solution.IsOpen && solution.Projects != null)
            {
                foreach (Project project in solution.Projects)
                {
                    // Check the cancel flag
                    if (this.cancel)
                        return;

                    Process(project);
                }
            }
        }
        
        /// <summary>
        /// Processes the specified project.
        /// </summary>
        public virtual void Process(Project project)
        {
            ProjectInfo projectInfo = new ProjectInfo(project);
            if (project != null && project.ProjectItems != null)
            {
                foreach (ProjectItem item in project.ProjectItems)
                {
                    // Check the cancel flag
                    if (this.cancel)
                        return;

                    Process(item, projectInfo);
                }
            }
        }

        /// <summary>
        /// Remove a Project
        /// </summary>
        /// <param name="project">The Project to remove</param>
        public virtual void Remove(Project project)
        { }

        /// <summary>
        /// Remove a Project Item
        /// </summary>
        /// <param name="item">The ProjectItem to remove</param>
        public virtual void Remove(ProjectItem item)
        { }

        /// <summary>
        /// Rename a Project
        /// </summary>
        /// <param name="project">The project to rename</param>
        /// <param name="oldName">The old name of the project</param>
        public virtual void Rename(Project project, string oldName)
        {
        }

        /// <summary>
        /// Rename a ProjectItem
        /// </summary>
        /// <param name="item">The item to rename</param>
        /// <param name="oldName">The old name of the item</param>
        public virtual void Rename(ProjectItem item, string oldName)
        {
        }

        #endregion
    }
}
