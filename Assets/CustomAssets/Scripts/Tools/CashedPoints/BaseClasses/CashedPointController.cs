using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyTools.Helpers.CashedPointUnderHood;

namespace MyTools.Helpers
{
    public struct PointInfo<TInfo>
    {
        public Vector3 position;
        public Quaternion rotation;
        public TInfo info;
    }
    
    public class LocalCashedPointController<TPointComponent, TInfo> : CashedPointControllerBase<TInfo> where TPointComponent : CashedPointBase<TInfo>
    {
        protected sealed override PointInfo<TInfo> CollectInfo(CashedPointBase<TInfo> cashedPoint)
        {
            PointInfo<TInfo> pointInfo;
            pointInfo.position = cashedPoint.TR.localPosition;
            pointInfo.rotation = cashedPoint.TR.localRotation;
            pointInfo.info = cashedPoint.info;
            return pointInfo;
        }
        protected sealed override void RestoreInfo(GameObject obj, PointInfo<TInfo> pointInfo)
        {
            var c = obj.AddComponent<TPointComponent>();
            c.TR.localPosition = pointInfo.position;
            c.TR.localRotation = pointInfo.rotation;
            c.info = pointInfo.info;
        }
    }
    public class GlobalCashedPointController<TPointComponent, TInfo> : CashedPointControllerBase<TInfo> where TPointComponent : CashedPointBase<TInfo>
    {
        protected sealed override PointInfo<TInfo> CollectInfo(CashedPointBase<TInfo> cashedPoint)
        {
            PointInfo<TInfo> pointInfo;
            pointInfo.position = cashedPoint.TR.position;
            pointInfo.rotation = cashedPoint.TR.rotation;
            pointInfo.info = cashedPoint.info;
            return pointInfo;
        }
        protected sealed override void RestoreInfo(GameObject obj, PointInfo<TInfo> pointInfo)
        {
            var c = obj.AddComponent<TPointComponent>();
            c.TR.position = pointInfo.position;
            c.TR.rotation = pointInfo.rotation;
            c.info = pointInfo.info;
        }
    }
}
