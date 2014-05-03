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
        
        #endregion
    }
}
