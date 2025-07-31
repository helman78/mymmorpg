using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Managers
{
    class MinimapManager : MonoSingleton<MinimapManager>
    {

        public UIMinimap minimap;
        private Collider minimapBoundingBox;
        public Collider MinimapBoundingBox
        {
            get { return this.minimapBoundingBox; }
        }
        public Sprite LoadCurMiniMap()
        {
            return Resloader.Load<Sprite>("UI/Minimap/" + User.Instance.CurrentMapData.MiniMap);
        }
        public void UpdateMinimap(Collider minimapBoundingBox)
        {
            this.minimapBoundingBox = minimapBoundingBox;
            if (this.minimap != null)
                this.minimap.UpdateMinimap();
        }
    }
}
