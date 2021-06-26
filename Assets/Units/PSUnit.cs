using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Man {
    public GameObject gameObject;
    public Vector2 relativePosition;

    public Man(Vector2 RelativePosition, Vector3 AbsolutePosition) {
        relativePosition = RelativePosition;
        var o = Resources.Load<GameObject>("Models/UnitDia_Prefab");
        gameObject = GameObject.Instantiate(o);
        gameObject.transform.position = AbsolutePosition;
    }
    public Man(Vector2 RelativePosition, Vector3 AbsolutePosition, string type) {
        relativePosition = RelativePosition;
        var o = Resources.Load<GameObject>("Models/UnitDia_Prefab_" + type);
        gameObject = GameObject.Instantiate(o);
        gameObject.transform.position = AbsolutePosition;
    }
}

public class LineInfo {
    public double ratio;
    public LineInfo() {

    }
}
public class SquareInfo {
    public int depth;
    public SquareInfo() {

    }
}
public class RingInfo {
    public double lineNum;
    public double lineRatio;
    public float lineSpace;
    public RingInfo() {

    }
}

public class PikeShotUnit {
    public int leftWingNum;
    public int rightWingNum;
    public int pikeNum;
    public PikeShotUnit() { }
}
public class ComboLineInfo {
    public double musketRatio;
    public float musketXSpace;
    public float musketZSpace;
    public double pikeRatio;
    public float pikeXSpace;
    public float pikeZSpace;
    public ComboLineInfo() { }
}
public class ComboRingInfo {
    public double pikeRatio;
    public float pikeXSpace;
    public float pikeZSpace;
    public double musketDepthRatio;
    public float musketXSpace;
    public float musketZSpace;
    public ComboRingInfo() { }
}
public class ComboSquareInfo {
    public int pikeDepth;
    public float pikeXSpace;
    public float pikeZSpace;
    public float musketXSpace;
    public float musketZSpace;
    public ComboSquareInfo() { }
}

public struct ForwardMove {
    public Vector2 startPos;
    public Vector2 endPos;
    public double startTime;

    public ForwardMove(Vector2 StartPos, Vector2 EndPos, double StartTime) {
        startPos = StartPos;
        endPos = EndPos;
        startTime = StartTime;
    }

}
public class PSUnit {

    public Vector2 pos;
    public List<Man> men;
    public Dictionary<string, List<Man>> subUnits;
    public double shapeWToHRatio;
    public double colsPercent;

    public GameObject ui;

    public List<object> moves;

    public double speed = 8.0;


    public PSUnit(Vector2 Pos, object Unit, object FormationInfo = null) {
        
        PSCore.OnUpdate += Update;


        pos = Pos;


        GameObject uiObj = Resources.Load<GameObject>("UI/PSUnitUi");
        ui = GameObject.Instantiate(uiObj);

        PSUnitUi uiScript = (PSUnitUi)GameObject.FindObjectOfType<PSUnitUi>();
        uiScript.psUnit = this;
        uiScript.unitPos = new Vector3(Pos.x, 200f, Pos.y);

        //SetUiPosition(Pos);
        

        // if (Unit is PikeShotUnit) { }
        PikeShotUnit pikeShotUnit = (PikeShotUnit)Unit;

        men = new List<Man>();
        subUnits = new Dictionary<string, List<Man>>();
        
        
        if (FormationInfo is ComboLineInfo) {
            ComboLineInfo comboLineInfo = (ComboLineInfo)FormationInfo;


            //// vars
            int width;
            int height;
            float xOffset;
            int lastRowNum;
            float lastRowOffset;
        
            int num;
            double ratio;
            float xSpace;
            float zSpace;

            //// pikes

            num = pikeShotUnit.pikeNum;
            ratio = comboLineInfo.pikeRatio;
            xSpace = comboLineInfo.pikeXSpace;
            zSpace = comboLineInfo.pikeZSpace;

            width = (int)Math.Round(Math.Pow( (double)num / ratio, 0.5 ) * ratio);
            height = (int)Math.Ceiling( (double)num / (double)width );

            xOffset = (width - 1) * xSpace / 2 * -1;

            lastRowNum = num % width;
            lastRowOffset = (lastRowNum == 0) ? 0 : (width - lastRowNum) / 2.0f;


            //Debug.Log(width);
            //Debug.Log("last row" + lastRowNum + " / " + width);

            for (int i = 1; i <= num; i++) {
                float x = xOffset + ((i - 1) % width) * xSpace;
                if (((i - 1) / width) == height - 1) x += (float)lastRowOffset * xSpace;
                //float y = 200.0f;
                float z = ((i-1)/width) * -zSpace;

                Vector2 relPos = new Vector2(x, z);
                Vector3 absPos = new Vector3(Pos.x + x, 200.0f, Pos.y + z);

                Man man = new Man( relPos, absPos, "Pike" );
                //Debug.Log( i.ToString() + " " + (i-1).ToString() + " " + ((i - 1) % width) + " " + ((i-1)/width) );
                men.Add(man);
            }

            float pikeHalfWidth = width / 2f * xSpace;
            float pikeHalfHeight = height / 2f * zSpace;


            // wings
            for (int w = 0; w <= 1; w++) {

                num = pikeShotUnit.leftWingNum;
                ratio = comboLineInfo.musketRatio;
                xSpace = comboLineInfo.musketXSpace;
                zSpace = comboLineInfo.musketZSpace;

                width = (int)Math.Round(Math.Pow( (double)num / ratio, 0.5 ) * ratio);
                height = (int)Math.Ceiling( (double)num / (double)width );

                xOffset = (width - 1) * xSpace / 2 * -1;

                lastRowNum = num % width;
                lastRowOffset = (lastRowNum == 0) ? 0 : (width - lastRowNum) / 2.0f;
            
                float musketHalfWidth = width / 2f * xSpace;
                float musketHalfHeight = height / 2f * zSpace;

                //Debug.Log(width);
                //Debug.Log("last row" + lastRowNum + " / " + width);

                for (int i = 1; i <= num; i++) {
                    float x;
                    if (w == 0) x = -pikeHalfWidth - musketHalfWidth;
                    else x = pikeHalfWidth + musketHalfWidth;
                    x += xOffset + ((i - 1) % width) * xSpace;
                    if (((i - 1) / width) == height - 1) x += (float)lastRowOffset * xSpace;
                    //float y = 200.0f;
                    float z = ((i-1)/width) * -zSpace;

                    Vector2 relPos = new Vector2(x, z);
                    Vector3 absPos = new Vector3(Pos.x + x, 200.0f, Pos.y + z);

                    Man man = new Man( relPos, absPos, "Musket" );
                    //Debug.Log( i.ToString() + " " + (i-1).ToString() + " " + ((i - 1) % width) + " " + ((i-1)/width) );
                    men.Add(man);
                }


            }


        }



        if (FormationInfo is ComboRingInfo) {
            ComboRingInfo comboRingInfo = (ComboRingInfo)FormationInfo;


            //// vars
            int width;
            int height;
            float xOffset;
            int lastRowNum;
            float lastRowOffset;
        
            int num;
            double ratio;
            float xSpace;
            float zSpace;

            //// pikes

            num = pikeShotUnit.pikeNum;
            ratio = comboRingInfo.pikeRatio;
            xSpace = comboRingInfo.pikeXSpace;
            zSpace = comboRingInfo.pikeZSpace;

            width = (int)Math.Round(Math.Pow( (double)num / ratio, 0.5 ) * ratio);
            height = (int)Math.Ceiling( (double)num / (double)width );

            xOffset = (width - 1) * xSpace / 2 * -1;

            lastRowNum = num % width;
            lastRowOffset = (lastRowNum == 0) ? 0 : (width - lastRowNum) / 2.0f;


            //Debug.Log(width);
            //Debug.Log("last row" + lastRowNum + " / " + width);

            for (int i = 1; i <= num; i++) {
                float x = Pos.x + xOffset + ((i - 1) % width) * xSpace;
                if (((i - 1) / width) == height - 1) x += (float)lastRowOffset * xSpace;
                //float y = 200.0f;
                float z = Pos.y + ((i-1)/width) * -zSpace;

                Vector2 relPos = new Vector2(x, z);
                Vector3 absPos = new Vector3(Pos.x + x, 200.0f, Pos.y + z);

                Man man = new Man( relPos, absPos, "Pike" );
                //Debug.Log( i.ToString() + " " + (i-1).ToString() + " " + ((i - 1) % width) + " " + ((i-1)/width) );
                men.Add(man);
            }

            int pikeWidth = width;
            int pikeHeight = height;
            float pikeHalfWidth = width / 2f * xSpace;
            float pikeHalfHeight = height / 2f * zSpace;

            // wings
            for (int w = 0; w <= 1; w++) {

                int menDeployed = 0;

                for (int n = 1; n <= 10; n++) { // n <= 10 just to prevent any infinite loop

                    num = pikeShotUnit.leftWingNum;
                    xSpace = comboRingInfo.musketXSpace;
                    zSpace = comboRingInfo.musketZSpace;

                    int width1 = (pikeWidth / 2) + (pikeWidth % 2) + n;
                    int width2 = (pikeWidth / 2) + n;
                    

                    for (int i = 1; i <= width1; i++) {

                        float x;
                        // start pos
                        if (w == 0) {
                            x = -pikeHalfWidth - (xSpace / 2f) - (n-1)*xSpace;
                            // iterate
                            x += ((i - 1) % width) * xSpace;
                        }
                        else {
                            x = pikeHalfWidth + (xSpace / 2f) + (n-1)*xSpace;
                            // iterate
                            x += ((i - 1) % width) * -xSpace;
                        }

                        //if (((i - 1) / width) == height - 1) x += (float)lastRowOffset * xSpace;

                        //float y = 200.0f;

                        float z = n * zSpace;
                        
                        Vector2 relPos = new Vector2(x, z);
                        Vector3 absPos = new Vector3(Pos.x + x, 200.0f, Pos.y + z);

                        Man man = new Man( relPos, absPos, "Musket" );
                        //Debug.Log( i.ToString() + " " + (i-1).ToString() + " " + ((i - 1) % width) + " " + ((i-1)/width) );
                        men.Add(man);

                        menDeployed++;
                        if (menDeployed == num) goto Wing_Complete;
                    }

                    for (int i = 1; i <= pikeHeight + (n-1)*2; i++) {

                        float x;
                        // start pos
                        if (w == 0) x = -pikeHalfWidth + (xSpace / 2f) - n*xSpace;
                        else x = pikeHalfWidth - (xSpace / 2f) + n*xSpace;
                        // iterate

                        //if (((i - 1) / width) == height - 1) x += (float)lastRowOffset * xSpace;

                        //float y = 200.0f;

                        float z = (n-1)*zSpace;
                        z += (i - 1) * -zSpace;
                        
                        Vector2 relPos = new Vector2(x, z);
                        Vector3 absPos = new Vector3(Pos.x + x, 200.0f, Pos.y + z);

                        Man man = new Man( relPos, absPos, "Musket" );
                        //Debug.Log( i.ToString() + " " + (i-1).ToString() + " " + ((i - 1) % width) + " " + ((i-1)/width) );
                        men.Add(man);

                        menDeployed++;
                        if (menDeployed == num) goto Wing_Complete;
                    }
                
                    for (int i = 1; i <= width1; i++) {

                        float x;
                        // start pos
                        if (w == 0) {
                            x = -pikeHalfWidth - (xSpace / 2f) - (n-1)*xSpace;
                            // move across one step
                            x += ((i - 1) % width) * xSpace;
                        }
                        else {
                            x = pikeHalfWidth + (xSpace / 2f) + (n-1)*xSpace;
                            // move across one step
                            x += ((i - 1) % width) * -xSpace;
                        }

                        //if (((i - 1) / width) == height - 1) x += (float)lastRowOffset * xSpace;

                        //float y = 200.0f;

                        float z = -pikeHalfHeight - pikeHalfHeight - (n-1) * zSpace;
                        
                        Vector2 relPos = new Vector2(x, z);
                        Vector3 absPos = new Vector3(Pos.x + x, 200.0f, Pos.y + z);

                        Man man = new Man( relPos, absPos, "Musket" );
                        //Debug.Log( i.ToString() + " " + (i-1).ToString() + " " + ((i - 1) % width) + " " + ((i-1)/width) );
                        men.Add(man);

                        menDeployed++;
                        if (menDeployed == num) goto Wing_Complete;
                    }
                }

                Wing_Complete: /*nothing*/;

            }


        }
        

        if (FormationInfo is ComboSquareInfo) {
            ComboSquareInfo comboSquareInfo = (ComboSquareInfo)FormationInfo;


            //// vars
            //int width;
            //int height;
            //float xOffset;
            //int lastRowNum;
            //float lastRowOffset;
        
            int num;
            int depth;
            //double ratio;
            float xSpace;
            float zSpace;

            //////// pikes
            num = pikeShotUnit.pikeNum;
            depth = comboSquareInfo.pikeDepth;
            float pikeXSpace = xSpace = comboSquareInfo.pikeXSpace;
            float pikeZSpace = zSpace = comboSquareInfo.pikeZSpace;
            
            int squareWidth = (int) Math.Floor( (double)num / 4.0 / depth + depth );
            int leftOversNum = num - ((squareWidth-depth) * depth * 4);
                
            float squareXOffset = (squareWidth - 1) / 2 * -1 * xSpace;
            
            // front
            for (int i = 1; i <= squareWidth * depth; i++) {
                float x = squareXOffset + ((i - 1) % squareWidth) * xSpace;
                //if (((i - 1) / width) == height - 1) x += (float)lastRowOffset * xSpace;
                //float y = 200.0f;
                float z = ((i-1)/squareWidth) * -zSpace;

                Vector2 relPos = new Vector2(x, z);
                Vector3 absPos = new Vector3(Pos.x + x, 200.0f, Pos.y + z);

                Man man = new Man( relPos, absPos, "Pike" );
                //Debug.Log( i.ToString() + " " + (i-1).ToString() + " " + ((i - 1) % squareWidth) + " " + ((i-1)/squareWidth) );
                men.Add(man);
            }

            //Debug.Log(leftOversNum);

            // left overs
            for (int i = 1; i <= leftOversNum; i++) {
                float x = squareXOffset + (depth + i - 1) * xSpace;
                if ( (double)i > (leftOversNum / 2.0) ) x += (squareWidth - depth*2 - leftOversNum) * xSpace;
                //float y = 200.0f;
                float z = depth * -zSpace;
                
                Vector2 relPos = new Vector2(x, z);
                Vector3 absPos = new Vector3(Pos.x + x, 200.0f, Pos.y + z);

                Man man = new Man( relPos, absPos, "Pike" );
                //Debug.Log( i.ToString() + " " + (i-1).ToString() + " " + ((i - 1) % squareWidth) + " " + ((i-1)/squareWidth) );
                men.Add(man);
            }
                
            // sides
            for (int i = 1; i <= (squareWidth - depth*2) * depth * 2; i++) {
                int xp = (i - 1) % (depth*2);
                float x = squareXOffset + xp * xSpace;
                if (xp >= depth) x += (squareWidth - depth*2) * xSpace;
                //float y = 200.0f;
                float z = (depth + (i-1)/(depth*2) ) * -zSpace;
                
                Vector2 relPos = new Vector2(x, z);
                Vector3 absPos = new Vector3(Pos.x + x, 200.0f, Pos.y + z);

                Man man = new Man( relPos, absPos, "Pike" );
                //Debug.Log( i.ToString() + " " + (i-1).ToString() + " " + ((i - 1) % squareWidth) + " " + ((i-1)/squareWidth) );
                men.Add(man);
            }

            // back
            for (int i = 1; i <= squareWidth * depth; i++) {
                float x = squareXOffset + ((i - 1) % squareWidth) * xSpace;
                //if (((i - 1) / width) == height - 1) x += (float)lastRowOffset * xSpace;
                //float y = 200.0f;
                //Debug.Log("Square Back " + (squareWidth-depth).ToString() + " " + ((i - 1) / squareWidth).ToString());
                float z = ((squareWidth-depth) + (i-1)/squareWidth) * -zSpace;
                
                Vector2 relPos = new Vector2(x, z);
                Vector3 absPos = new Vector3(Pos.x + x, 200.0f, Pos.y + z);

                Man man = new Man( relPos, absPos, "Pike" );
                //Debug.Log( i.ToString() + " " + (i-1).ToString() + " " + ((i - 1) % squareWidth) + " " + ((i-1)/squareWidth) );
                men.Add(man);
            }

            int pikeWidth = squareWidth;
            int pikeDepth = depth;
            

            // wings
            for (int w = 0; w <= 1; w++) {

                int menDeployed = 0;

                for (int n = 1; n <= 10; n++) { // n <= 10 just to prevent any infinite loop

                    num = pikeShotUnit.leftWingNum;
                    xSpace = comboSquareInfo.musketXSpace;
                    zSpace = comboSquareInfo.musketZSpace;

                    int width1 = (pikeWidth - pikeDepth - pikeDepth) / 2 + (pikeWidth % 2) /*- (n-1)*/;
                    int width2 = (pikeWidth - pikeDepth - pikeDepth) / 2 /*- (n-1)*/;

                    
                    int topNum = (w == 0) ? width1 : width2;
                    for (int i = 1; i <= topNum - (n-1); i++) {

                        float x;
                        // start pos
                        if (w == 0) {
                            x = -(pikeWidth/2.0f - pikeDepth - 0.5f)*pikeXSpace + (n-1)*xSpace;
                            // iterate
                            x += (i - 1) * xSpace;
                        }
                        else {
                            x = (pikeWidth/2.0f - pikeDepth - 0.5f)*pikeXSpace - (n-1)*xSpace;
                            // iterate
                            x += (i - 1) * -xSpace;
                        }

                        //if (((i - 1) / width) == height - 1) x += (float)lastRowOffset * xSpace;

                        //float y = 200.0f;

                        float z = -pikeDepth*pikeZSpace - (n-1)*zSpace;
                        
                        Vector2 relPos = new Vector2(x, z);
                        Vector3 absPos = new Vector3(Pos.x + x, 200.0f, Pos.y + z);

                        Man man = new Man( relPos, absPos, "Musket" );
                        //Debug.Log( i.ToString() + " " + (i-1).ToString() + " " + ((i - 1) % width) + " " + ((i-1)/width) );
                        men.Add(man);

                        menDeployed++;
                        if (menDeployed == num) goto Wing_Complete;
                   }

                   for (int i = 1; i <= pikeWidth - pikeDepth - pikeDepth - (n)*2; i++) {

                        float x;
                        // start pos
                        if (w == 0) x = Pos.x - (pikeWidth/2f - pikeDepth + 0.5f)*pikeXSpace + (n)*xSpace;
                        else x = Pos.x + (pikeWidth/2f - pikeDepth + 0.5f)*pikeXSpace - (n)*xSpace;
                        // iterate

                        //if (((i - 1) / width) == height - 1) x += (float)lastRowOffset * xSpace;

                        //float y = 200.0f;

                        float z = Pos.y - (pikeDepth + n)*zSpace - (i-1)*zSpace;
                        
                        Vector2 relPos = new Vector2(x, z);
                        Vector3 absPos = new Vector3(Pos.x + x, 200.0f, Pos.y + z);

                        Man man = new Man( relPos, absPos );
                        //Debug.Log( i.ToString() + " " + (i-1).ToString() + " " + ((i - 1) % width) + " " + ((i-1)/width) );
                        men.Add(man);

                        menDeployed++;
                        if (menDeployed == num) goto Wing_Complete;
                    }

                    int bottomNum = (w == 0) ? width2 : width1;
                    for (int i = 1; i <= bottomNum - (n-1); i++) {

                        float x;
                        // start pos
                        if (w == 0) {
                            x = Pos.x - (pikeWidth/2.0f - pikeDepth - 0.5f)*pikeXSpace + (n-1)*xSpace;
                            // iterate
                            x += (i - 1) * xSpace;
                        }
                        else {
                            x = Pos.x + (pikeWidth/2.0f - pikeDepth - 0.5f)*pikeXSpace - (n-1)*xSpace;
                            // iterate
                            x += (i - 1) * -xSpace;
                        }

                        //if (((i - 1) / width) == height - 1) x += (float)lastRowOffset * xSpace;

                        //float y = 200.0f;

                        float z = Pos.y - (pikeWidth - pikeDepth)*pikeZSpace + n*zSpace;
                        
                        Vector2 relPos = new Vector2(x, z);
                        Vector3 absPos = new Vector3(Pos.x + x, 200.0f, Pos.y + z);

                        Man man = new Man( relPos, absPos, "Musket" );
                        //Debug.Log( i.ToString() + " " + (i-1).ToString() + " " + ((i - 1) % width) + " " + ((i-1)/width) );
                        men.Add(man);

                        menDeployed++;
                        if (menDeployed == num) goto Wing_Complete;
                    }
                }

                Wing_Complete: ;

            }
            

        }
        
    }

    /*public PSUnit(Vector2 Pos, int num, double ShapeWToHRatio, float Space, string Formation = "Line", object FormationInfo = null) {

        pos = Pos;
        
        float space = Space;
        float xSpace = 0f;
        float zSpace = 0f;
        
        men = new List<Man>();

        int width;
        int height;
        float xOffset;
        int lastRowNum;
        float lastRowOffset;

        switch(Formation) {
            case "Line":
                shapeWToHRatio = ShapeWToHRatio; // 1.6;
        
                xSpace = space;
                zSpace = space;

                width = (int)Math.Round(Math.Pow( (double)num / shapeWToHRatio, 0.5 ) * shapeWToHRatio);
                height = (int)Math.Ceiling( (double)num / (double)width );

                xOffset = (width - 1) * xSpace / 2 * -1;

                lastRowNum = num % width;
                lastRowOffset = (lastRowNum == 0) ? 0 : (width - lastRowNum) / 2.0f;


                //Debug.Log(width);
                //Debug.Log("last row" + lastRowNum + " / " + width);

                for (int i = 1; i <= num; i++) {
                    float x = Pos.x + xOffset + ((i - 1) % width) * xSpace;
                    if (((i - 1) / width) == height - 1) x += (float)lastRowOffset * xSpace;
                    float y = 200.0f;
                    float z = Pos.y + ((i-1)/width) * -zSpace;
                    Man man = new Man( new Vector3(x, y, z) );
                    //Debug.Log( i.ToString() + " " + (i-1).ToString() + " " + ((i - 1) % width) + " " + ((i-1)/width) );
                    men.Add(man);
                }
                break;

            case "Ring_Left":

                RingInfo ringInfo = (RingInfo)FormationInfo;
                
                int lineWidth = (int)Math.Round(Math.Pow( (double)ringInfo.lineNum / ringInfo.lineRatio, 0.5 ) * ringInfo.lineRatio);
                int lineHeight = (int)Math.Ceiling( (double)ringInfo.lineNum / (double)lineWidth );
                //ringInfo.lineSpace
        
                xSpace = space;
                zSpace = space;

                width = (int)Math.Round(Math.Pow( (double)num / shapeWToHRatio, 0.5 ) * shapeWToHRatio);
                height = (int)Math.Ceiling( (double)num / (double)width );

                xOffset = (width - 1) * xSpace / 2 * -1;

                lastRowNum = num % width;
                lastRowOffset = (lastRowNum == 0) ? 0 : (width - lastRowNum) / 2.0f;


                //Debug.Log(width);
                //Debug.Log("last row" + lastRowNum + " / " + width);

                for (int i = 1; i <= num; i++) {
                    float x = Pos.x + xOffset + ((i - 1) % width) * xSpace;
                    if (((i - 1) / width) == height - 1) x += (float)lastRowOffset * xSpace;
                    float y = 200.0f;
                    float z = Pos.y + ((i-1)/width) * -zSpace;
                    Man man = new Man( new Vector3(x, y, z) );
                    //Debug.Log( i.ToString() + " " + (i-1).ToString() + " " + ((i - 1) % width) + " " + ((i-1)/width) );
                    men.Add(man);
                }
                break;


            case "Square":

                int squareDepth = 2;
                int squareWidth = (int) Math.Floor( (double)num / 4.0 / squareDepth + squareDepth );
                int leftOversNum = num - ((squareWidth-squareDepth) * squareDepth * 4);
                
                xSpace = space;
                zSpace = space;

                //men = new List<Man>();
                //int width = (int)Math.Round(Math.Pow( (double)num / shapeWToHRatio, 0.5 ) * shapeWToHRatio);
                //int height = (int)Math.Ceiling( (double)num / (double)width );

                float squareXOffset = (squareWidth - 1) / 2 * -1 * xSpace;

                //int lastRowNum = num % width;
                //float lastRowOffset = (width - lastRowNum) / 2.0f;


                //Debug.Log(width);
                //Debug.Log("last row" + lastRowNum + " / " + width);
                
                // front
                for (int i = 1; i <= squareWidth * squareDepth; i++) {
                    float x = Pos.x + squareXOffset + ((i - 1) % squareWidth) * xSpace;
                    //if (((i - 1) / width) == height - 1) x += (float)lastRowOffset * xSpace;
                    float y = 200.0f;
                    float z = Pos.y + ((i-1)/squareWidth) * -zSpace;
                    Man man = new Man( new Vector3(x, y, z) );
                    //Debug.Log( i.ToString() + " " + (i-1).ToString() + " " + ((i - 1) % squareWidth) + " " + ((i-1)/squareWidth) );
                    men.Add(man);
                }

                Debug.Log(leftOversNum);

                // left overs
                for (int i = 1; i <= leftOversNum; i++) {
                    float x = Pos.x + squareXOffset + (squareDepth + i - 1) * xSpace;
                    if ( (double)i > (leftOversNum / 2.0) ) x += (squareWidth - squareDepth*2 - leftOversNum) * xSpace;
                    float y = 200.0f;
                    float z = Pos.y + squareDepth * -zSpace;
                    Man man = new Man( new Vector3(x, y, z) );
                    //Debug.Log( i.ToString() + " " + (i-1).ToString() + " " + ((i - 1) % squareWidth) + " " + ((i-1)/squareWidth) );
                    men.Add(man);
                }
                
                // sides
                for (int i = 1; i <= (squareWidth - squareDepth*2) * squareDepth * 2; i++) {
                    int xp = (i - 1) % (squareDepth*2);
                    float x = Pos.x + squareXOffset + xp * xSpace;
                    if (xp >= squareDepth) x += (squareWidth - squareDepth*2) * xSpace;
                    float y = 200.0f;
                    float z = Pos.y + (squareDepth + (i-1)/(squareDepth*2) ) * -zSpace;
                    Man man = new Man( new Vector3(x, y, z) );
                    //Debug.Log( i.ToString() + " " + (i-1).ToString() + " " + ((i - 1) % squareWidth) + " " + ((i-1)/squareWidth) );
                    men.Add(man);
                }

                // back
                for (int i = 1; i <= squareWidth * squareDepth; i++) {
                    float x = Pos.x + squareXOffset + ((i - 1) % squareWidth) * xSpace;
                    //if (((i - 1) / width) == height - 1) x += (float)lastRowOffset * xSpace;
                    float y = 200.0f;
                    float z = Pos.y + ((squareWidth-2) + (i-1)/squareWidth) * -zSpace;
                    Man man = new Man( new Vector3(x, y, z) );
                    //Debug.Log( i.ToString() + " " + (i-1).ToString() + " " + ((i - 1) % squareWidth) + " " + ((i-1)/squareWidth) );
                    men.Add(man);
                }
                

                break;
        }

    }*/
    
    public void MoveTo(Vector2 dest) {
        moves = new List<object> { new ForwardMove(pos, dest, Time.time) };

        Debug.Log(moves.Count);
    }

    /*
    // Start is called before the first frame update
    void Start()
    {
        
    }
    */

    // Update is called once per frame
    void Update()
    {
        if (moves != null && moves.Count > 0) {

            if (moves[0] is ForwardMove) {
                ForwardMove fm = (ForwardMove)moves[0];

                Vector2 vector = fm.endPos - fm.startPos;
                vector.Normalize();
                pos = fm.startPos + vector * (float)((Time.time - fm.startTime) * speed);

                if ( (pos - fm.startPos).magnitude > (fm.endPos- fm.startPos).magnitude ) {
                    pos = fm.endPos;
                    moves.RemoveAt(0);
                }

                UpdateMenPositions();
            }
        }
    }

    void UpdateMenPositions() {
        foreach (Man man in men) {
            man.gameObject.transform.position = new Vector3(pos.x + man.relativePosition.x, 200f, pos.y + man.relativePosition.y);
        }
    }

}