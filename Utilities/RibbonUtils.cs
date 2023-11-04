using System;
using System.Collections.Generic;
using System.Linq;
using AW = Autodesk.Windows;

namespace WT.Revit.Utilities
{
    public static class RibbonUtils
    {
        public static AW.RibbonTab GetTab(string tabName)
        {
            AW.RibbonControl ribbon = AW.ComponentManager.Ribbon;
            foreach (AW.RibbonTab tab in ribbon.Tabs)
            {
                if (tab.Id == tabName)
                {
                    return tab;
                }
            }
            return null;
        }
        public static AW.RibbonPanel GetPanel(string tabName, string panelName)
        {
            AW.RibbonTab ribbonTab = GetTab(tabName);

            foreach (AW.RibbonPanel ribbonPanel in ribbonTab.Panels)
            {
                if (ribbonPanel.Source.Id == "CustomCtrl_%" + tabName + "%" + panelName)
                {
                    return ribbonPanel;
                }
                else if (ribbonPanel.Source.Id == panelName)
                {
                    return ribbonPanel;
                }
            }
            return null;
        }
        public static AW.RibbonItem GetButton(string tabName, string panelName, string itemName)
        {
            AW.RibbonPanel ribbonPanel = GetPanel(tabName, panelName);
            foreach (AW.RibbonItem ribbonItem in ribbonPanel.Source.Items)
            {
                if (ribbonItem.Id == "CustomCtrl_%CustomCtrl_%" + tabName + "%" + panelName + "%" + itemName)
                {
                    return ribbonItem;
                }
                else if (ribbonItem.Text == itemName)
                {
                    return ribbonItem;
                }
                else if (ribbonItem.Id == itemName)
                {
                    return ribbonItem;
                }
            }
            throw new ArgumentNullException("GetButton", "Can not get Button");
        }
        public static IEnumerable<AW.RibbonItem> GetButtons(string tabName, string panelName, string itemName)
        {
            AW.RibbonPanel ribbonPanel = GetPanel(tabName, panelName);
            foreach (AW.RibbonItem ribbonItem in ribbonPanel.Source.Items)
            {
                if (ribbonItem.Id == "CustomCtrl_%CustomCtrl_%" + tabName + "%" + panelName + "%" + itemName)
                {
                    yield return ribbonItem;
                }
                else if (ribbonItem.Text == itemName)
                {
                    yield return ribbonItem;
                }
                else if (ribbonItem.Id == itemName)
                {
                    yield return ribbonItem;
                }
            }
        }
        public static void RemoveButton(string tabName, string panelName, string itemName)
        {
            AW.RibbonPanel ribbonPanel = GetPanel(tabName, panelName);
            List<AW.RibbonItem> ribbonItemsToRemove = GetButtons(tabName, panelName, itemName).ToList();
            if (ribbonItemsToRemove.Any())
            {
                foreach (AW.RibbonItem ribbonItem in ribbonItemsToRemove)
                {
                    ribbonPanel.Source.Items.Remove(ribbonItem);
                }
            }
            else throw new ArgumentNullException();
        }
        public static void HideButton(string tabName, string panelName, string itemName)
        {
            //AW.RibbonPanel ribbonPanel = GetPanel(tabName, panelName);
            List<AW.RibbonItem> ribbonItemsToHide = GetButtons(tabName, panelName, itemName).ToList();
            foreach (AW.RibbonItem ribbonItem in ribbonItemsToHide)
            {
                ribbonItem.IsVisible = false;
                ribbonItem.IsEnabled = false;
            }
        }
        public static AW.RibbonItem GetItem(this AW.RibbonRowPanel ribbonRowPanel, string ribbonItemName)
        {
            foreach (AW.RibbonItem ribbonItem in ribbonRowPanel.Items)
            {
                if (ribbonItem.Text == ribbonItemName || ribbonItem.Id == ribbonItemName)
                {
                    return ribbonItem;
                }
            }
            return null;
        }
        public static void ReplaceProperties(this AW.RibbonItem ribbonItem, AW.RibbonItem ribbonItemInfo)
        {
            ribbonItem.Image = ribbonItemInfo.Image;
            ribbonItem.LargeImage = ribbonItemInfo.LargeImage;
            ribbonItem.Size = ribbonItemInfo.Size;
            ribbonItem.ShowText = ribbonItemInfo.ShowText;
        }
        public static void Remove(this AW.RibbonRowPanel ribbonRowPanel, string ribbonItemName)
        {
            List<AW.RibbonItem> ribbonItemsToRemove = new List<AW.RibbonItem>();
            foreach (AW.RibbonItem ribbonItem in ribbonRowPanel.Items)
            {
                if (ribbonItem.Text == ribbonItemName || ribbonItem.Id == ribbonItemName)
                {
                    ribbonItemsToRemove.Add(ribbonItem);
                }
            }
            foreach (AW.RibbonItem ribbonItem in ribbonItemsToRemove)
            {
                ribbonRowPanel.Items.Remove(ribbonItem);
            }
        }
        public static AW.RibbonItem GetItem(this AW.RibbonSplitButton ribbonSplitButton, string automationName)
        {
            foreach (AW.RibbonItem ribbonItem in ribbonSplitButton.Items)
            {
                if (ribbonItem.AutomationName == automationName)
                {
                    return ribbonItem;
                }
            }
            return null;
        }
        public static IEnumerable<AW.RibbonItem> GetItems(this AW.RibbonSplitButton ribbonSplitButton, string automationName)
        {
            foreach (AW.RibbonItem ribbonItem in ribbonSplitButton.Items)
            {
                if (ribbonItem.AutomationName == automationName)
                {
                    yield return ribbonItem;
                }
            }
        }
        public static bool Contains(this AW.RibbonSplitButton ribbonSplitButton, AW.RibbonItem ribbonItem)
        {
            foreach (AW.RibbonItem ribbonItemz in ribbonSplitButton.Items)
            {
                if (ribbonItemz.AutomationName == ribbonItem.AutomationName)
                {
                    return true;
                }
            }
            return false;
        }
        public static bool Contains(this AW.RibbonSplitButton ribbonSplitButton, string ribbonItemName)
        {
            foreach (AW.RibbonItem ribbonItem in ribbonSplitButton.Items)
            {
                if (ribbonItem.Text == ribbonItemName)
                {
                    return true;
                }
            }
            return false;
        }
        public static bool Swap(this AW.RibbonRowPanel ribbonRowPanel, int x, int y)
        {
            if (ribbonRowPanel.Items.Count <= y || ribbonRowPanel.Items.Count <= x) return false;
            // swap index x and y
            (ribbonRowPanel.Items[y], ribbonRowPanel.Items[x]) = (ribbonRowPanel.Items[x], ribbonRowPanel.Items[y]);
            return true;
        }
    }
}
