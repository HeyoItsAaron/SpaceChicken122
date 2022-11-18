using System.Collections.Generic;
using UnityEngine;
using VRShooterKit.WeaponSystem;

namespace VRShooterKit
{
    /// <summary>
    /// Code to keep track of the arrow connections and avoid jittering
    /// </summary>
    public static class ArrowConnections
    {
        private static Dictionary<Rigidbody, List<ArrowConnectionInfo>> arrowConnections = new Dictionary<Rigidbody, List<ArrowConnectionInfo>>();
        private const int MAX_CONNECTIONS_PER_BODY = 3;


        public static void Add(Rigidbody rb , Arrow arrow)
        {
            
            List<ArrowConnectionInfo> infoList;
            if (arrowConnections.TryGetValue( rb, out infoList ))
            {               
                int index = GetConnectionIndexByArrow(infoList , arrow);

                //the connection already exist
                if (index != -1)
                    return;

                infoList.Add(new ArrowConnectionInfo(arrow));
                CheckConnections(infoList);
            }
            else
            {
                //create the connection
                arrowConnections.Add( rb , new List<ArrowConnectionInfo>() { new ArrowConnectionInfo(arrow) } );
            }
            
        }

        public static void Remove(Rigidbody rb , Arrow arrow)
        {
            List<ArrowConnectionInfo> infoList;
            if (arrowConnections.TryGetValue( rb, out infoList ))
            {
                int index = GetConnectionIndexByArrow( infoList, arrow );

                if (index != -1)
                {
                    infoList.RemoveAt(index);
                }

            }
        }

        public static void MoveArrowConnections(Rigidbody from , Rigidbody to)
        {

            List<ArrowConnectionInfo> connections = null;

            if (arrowConnections.TryGetValue( from, out connections ))
            {
                for ( int n = 0; n < connections.Count; n++ )
                {                   
                    connections[n].arrow.StickToRigidBody(to);
                    Remove(from , connections[n].arrow);
                }
            }

        }

        private static int GetConnectionIndexByArrow(List<ArrowConnectionInfo> infoList , Arrow arrow)
        {
            //check if the connection exist
            for (int n = 0; n < infoList.Count; n++)
            {
                if (infoList[n].arrow == arrow)
                {
                    return n;
                }
            }

            return -1;
        }

        private static void CheckConnections(List<ArrowConnectionInfo> infoList)
        {
            if (infoList.Count <= MAX_CONNECTIONS_PER_BODY)
                return;

            int index = 0;
            float t = float.MaxValue;
            for (int n = 0; n < infoList.Count; n++)
            {
                if (infoList[n].time < t)
                {
                    t = infoList[n].time;
                    index = n;
                }
            }

            MonoBehaviour.Destroy( infoList[index].arrow.gameObject );
            infoList.RemoveAt(index);
        }
       
    }

    public class ArrowConnectionInfo
    {
        public Arrow arrow = null;
        public float time = 0.0f;

        public ArrowConnectionInfo( Arrow arrow)
        {
            this.arrow = arrow;
            time = Time.time;
        }

        public ArrowConnectionInfo() { }
    }

}

