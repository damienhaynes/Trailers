using System.Collections.Generic;
using MediaPortal.GUI.Library;
using MediaPortal.Dialogs;
using Trailers.Localisation;

namespace Trailers.GUI
{
    public class GUIDialogTrailers : GUIDialogMenu
    {
        #region Overrides

        public override bool Init()
        {
            return Load(GUIGraphicsContext.Skin + @"\Trailers.Selection.Menu.xml");
        }

        public override int GetID
        {
            get
            {
                return 11898;
            }
        }

        public override string GetModuleName()
        {
            return Translation.Trailers;
        }

        public override void DoModal(int dwParentId)
        {
            // override hard-coded label in ID: 5
            GUIControl.SetControlLabel(this.GetID, 5, Translation.Trailers);

            base.DoModal(dwParentId);
        }

        #endregion

    }
}
