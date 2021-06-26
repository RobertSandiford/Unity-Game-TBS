using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PSCore : MonoBehaviour
{

    public static System.Action OnUpdate;


    // Start is called before the first frame update
    void Start()
    {

        PSController.Start();
        OnUpdate += PSController.Update;

        /*
        public class PikeShotUnit {
            public int leftWingNum;
            public int rightWingNum;
            public int pikeNum;
            public PikeShotUnit() { }
        }
        public class ComboLineInfo {
            public double musketRatio;
            public float mustketXSpace;
            public float mustketZSpace;
            public double pikeRatio;
            public float pikeXSpace;
            public float pikeZSpace;
            public ComboLineInfo() { }
        }
        */


        PSUnit psUnit = new PSUnit(
            new Vector2(2200, 1800),
            new PikeShotUnit { pikeNum = 200, leftWingNum = 100, rightWingNum = 100 },

            //new ComboLineInfo { pikeRatio = 1.0, pikeXSpace = 22f, pikeZSpace = 22f, musketRatio = 1.0, musketXSpace = 22f, musketZSpace = 22f }
            new ComboLineInfo { pikeRatio = 3.2, pikeXSpace = 22f, pikeZSpace = 22f, musketRatio = 2.7, musketXSpace = 22f, musketZSpace = 22f }

            //new ComboRingInfo { pikeRatio = 1.0, pikeXSpace = 22f, pikeZSpace = 22f, musketXSpace = 22f, musketZSpace = 22f, }
            //new ComboSquareInfo { pikeDepth = 2, pikeXSpace = 22f, pikeZSpace = 22f, musketXSpace = 22f, musketZSpace = 22f,  }
        );

        //ShowTime("Map Post Initialisation");
        
       /* PSUnit psUnit1 = new PSUnit(new Vector2(0, 0), 102, 1, 26f, "Ring_Left", new RingInfo{ lineNum=200, lineRatio=100, lineSpace=22f } );
        PSUnit psUnit2 = new PSUnit(new Vector2(400, 0), 200, 1, 22f, "Line");
        PSUnit psUnit3 = new PSUnit(new Vector2(800, 0), 100, 1, 26f, "Line");
        
        public class PikeShotUnit {
            public int leftWingNum;
            public int rightWingNum;
            public int pikeNum;
            public PikeShotUnit() { }
        }
        public class ComboLineInfo {
            public double musketRatio;
            public float mustketXSpace;
            public float mustketZSpace;
            public double pikeRatio;
            public float pikeXSpace;
            public float pikeZSpace;
            public ComboLineInfo() { }
        }*/


    }

    // Update is called once per frame
    void Update()
    {
         if( OnUpdate != null )
            OnUpdate();
    }
}
