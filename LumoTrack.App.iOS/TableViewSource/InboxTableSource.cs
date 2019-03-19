using System;
using Foundation;
using LumoTrack.App.iOS.TableViewCell;
using LumoTrack.DTO;
using UIKit;

namespace LumoTrack.App.iOS.TableViewSource
{
    public class InboxTableSource : UITableViewSource
    {
        InboxDTO[] TableItems;



        public string CellID
        {
            get { return "InboxCellID"; }
        }

        public InboxTableSource(InboxDTO[] items)
        {
            TableItems = items;
        }

        public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {
            var cell = tableView.DequeueReusableCell(CellID, indexPath) as InboxViewCell;
            var item = TableItems[indexPath.Row];

            cell.SetView(item);

            return cell;
        }

        public override nint RowsInSection(UITableView tableview, nint section)
        {
            return TableItems.Length;
        }
    }
}
