//
// Authors:	 
//	  Ivan N. Zlatev (contact i-nZ.net)
//
// (C) 2007-2008 Ivan N. Zlatev

//
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
// 
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;

using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Reflection;
using System.ComponentModel.Design;

namespace alfrmdesign
{
	public partial class PropertyGrid : UserControl
	{
		private IServiceProvider _serviceProvider;
		private bool _updating;

		public PropertyGrid()
		{
			InitializeComponent();
		}

		private void OnComponentsCombo_SelectedIndexChanged (object sender, EventArgs args)
		{
            try
            {
                if (_componentsCombo.SelectedIndex != -1 && !_updating)
                    SetPrimarySelection((string)_componentsCombo.Items[_componentsCombo.SelectedIndex]);
            }
            catch
            {

            }
		}

		private void SetPrimarySelection (string componentName)
		{try{
			ISelectionService selectionService = this.GetService (typeof (ISelectionService)) as ISelectionService;
			IContainer container = this.GetService (typeof (IContainer)) as IContainer;
			if (selectionService == null || container == null)
				return;

			IComponent selectedComponent = container.Components[componentName];
 
         // selectionService.PrimarySelection
			selectionService.SetSelectedComponents (new IComponent[] { selectedComponent });
              }
            catch
            {

            }
		}

		public void Clear ()
		{
			Update (null);
		}

		public void Update (IServiceProvider serviceProvider)
		{
            try
            {
                if (serviceProvider == null)
                {
                    _propertyGrid.SelectedObject = null;
                    _componentsCombo.Items.Clear();
                }
                else
                {
                    DisableComponentsChangeNotification(_serviceProvider);
                    UpdatePropertyGrid(serviceProvider);
                    PopulateComponentsList(serviceProvider);
                    EnableComponentsChangeNotification(serviceProvider);
                }
                _serviceProvider = serviceProvider;
            }
            catch
            {

            }
		}

		private void UpdatePropertyGrid (IServiceProvider serviceProvider)
		{
            try
            {
                _propertyGrid.SelectedObject = null;
                if (serviceProvider == null)
                    return;
                ISelectionService selectionService = serviceProvider.GetService(typeof(ISelectionService)) as ISelectionService;
                if (selectionService == null)
                    return;

                //ICollection selectionCollection = selectionService.GetSelectedComponents ();
                //if (selectionCollection != null) {
                //    object[] selection = new object[selectionCollection.Count];
                //    selectionCollection.CopyTo (selection, 0);
                //    _propertyGrid.SelectedObjects = selection;
                //    ShowEventsTab ();


                if (selectionService.PrimarySelection != null)
                {
                    _propertyGrid.SelectedObject = selectionService.PrimarySelection;
                    ShowEventsTab();

                }
            }
            catch
            {

            }
		}

		private void PopulateComponentsList (IServiceProvider serviceProvider)
		{
            try{
			_componentsCombo.Items.Clear ();

			if (serviceProvider == null)
				return;
			ISelectionService selectionService = serviceProvider.GetService (typeof (ISelectionService)) as ISelectionService;
			IContainer container = serviceProvider.GetService (typeof (IContainer)) as IContainer;
			if (selectionService == null || container == null || container.Components == null)
				return;

			int primaryIndex = -1;
			for (int i=0; i < container.Components.Count; i++) {
				_componentsCombo.Items.Add (container.Components[i].Site.Name);
				if (selectionService != null && container.Components[i] == selectionService.PrimarySelection)
					primaryIndex = i;
			}
			if (primaryIndex != -1) {
				_updating = true; // in order to ignore the raised selectedindexchanged
				_componentsCombo.SelectedIndex = primaryIndex;
				_updating = false;
			}
            }
            catch
            {

            }
		}

		// MWF's PropertyGrid doesn't support EventsTab
		// 
		private void ShowEventsTab ()
		{
            try{
			IComponent component = _propertyGrid.SelectedObject as IComponent;
			if (component != null) {
				_propertyGrid.Site = component.Site;

				_propertyGrid.PropertyTabs.AddTabType (typeof (System.Windows.Forms.Design.EventsTab));
				_propertyGrid.GetType ().InvokeMember ("ShowEventsButton", BindingFlags.InvokeMethod | BindingFlags.Instance | BindingFlags.NonPublic,
								   null, _propertyGrid, new object [] { true });
			}
            }
            catch
            {

            }
		}

		private void OnPrimarySelectionChanged (object sender, EventArgs args)
		{
            try{
			ISelectionService selectionService = this.GetService (typeof (ISelectionService)) as ISelectionService;
			IContainer container = this.GetService (typeof (IContainer)) as IContainer;
			if (container == null || selectionService == null)
				return;

			_updating = true;
			IComponent primarySelection = selectionService.PrimarySelection as IComponent;
			if (primarySelection != null && primarySelection.Site != null && 
			    primarySelection.Site.Name != null)
				_componentsCombo.SelectedItem = primarySelection.Site.Name;
			UpdatePropertyGrid (_serviceProvider);
			_updating = false;
            }
            catch
            {

            }
		}

		private void EnableComponentsChangeNotification (IServiceProvider provider)
		{
            try{
			if (provider == null)
				return;
			IComponentChangeService changeService = provider.GetService (typeof (IComponentChangeService)) as IComponentChangeService;
			if (changeService != null) {
				changeService.ComponentAdded += new ComponentEventHandler (OnComponentAdded);
				changeService.ComponentRemoving += new ComponentEventHandler (OnComponentRemoving);
				changeService.ComponentRename += new ComponentRenameEventHandler (OnComponentRename);
			}
			ISelectionService selectionService = provider.GetService (typeof (ISelectionService)) as ISelectionService;
			if (selectionService != null)
				selectionService.SelectionChanged += new EventHandler (OnPrimarySelectionChanged);
            }
            catch
            {

            }
		}

		private void DisableComponentsChangeNotification (IServiceProvider provider)
		{
            try{
			if (provider == null)
				return;
			IComponentChangeService changeService = provider.GetService (typeof (IComponentChangeService)) as IComponentChangeService;
			if (changeService != null) {
				changeService.ComponentAdded -= new ComponentEventHandler (OnComponentAdded);
				changeService.ComponentRemoving -= new ComponentEventHandler (OnComponentRemoving);
				changeService.ComponentRename -= new ComponentRenameEventHandler (OnComponentRename);
			}
			ISelectionService selectionService = provider.GetService (typeof (ISelectionService)) as ISelectionService;
			if (selectionService != null)
				selectionService.SelectionChanged -= new EventHandler (OnPrimarySelectionChanged);
            }
            catch
            {

            }
		}


		private void OnComponentAdded (object sender, ComponentEventArgs args)
		{
            try{
			if (args.Component != null && args.Component.Site != null && 
			    args.Component.Site.Name != null) {
				_updating = true;
				_componentsCombo.Items.Add (args.Component.Site.Name);
				_updating = false;
			}
            }
            catch
            {

            }
		}

		private void OnComponentRemoving (object sender, ComponentEventArgs args)
		{
            try{
			if (args.Component != null && args.Component.Site != null && 
			    args.Component.Site.Name != null) {
				_updating = true;
				_componentsCombo.Items.Remove (args.Component.Site.Name);
				_updating = false;
			}
            }
            catch
            {

            }
		}

		private void OnComponentRename (object sender, ComponentRenameEventArgs args)
		{
            try{
			if (args.OldName == null || args.NewName == null)
				return;
			int toRename = _componentsCombo.Items.IndexOf (args.OldName);
			if (toRename != -1) {
				_updating = true;
				_componentsCombo.Items[toRename] = args.NewName;
				_updating = false;
			}
            }
            catch
            {

            }
		}

		protected override object GetService (Type type)
		{
            try{
			if (_serviceProvider != null)
				return _serviceProvider.GetService (type);
           
            }
            catch
            {
             
            }
            return null;
		}
    }
}
