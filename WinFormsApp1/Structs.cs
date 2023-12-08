﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MKE
{
    public struct Triangle
    {
        public PointD FirstVertex { get; set; }
        public PointD SecondVertex { get; set; }
        public PointD ThirdVertex { get; set; }

        public int FirstNodeNum { get; set; }
        public int SecondNodeNum { get; set; }
        public int ThirdNodeNum { get; set; }
    }

    public struct EdgeAndTriangle
    {
        public int NumTriangle { get; set; }
        public int NumEdgeTriangle { get; set; }
    }

    public struct GridNode
    {
        public PointD Node { get; set; }
        public int NumNode { get; set; }
    }

    public struct BorderAndNode
    {
        public int NumBorder { get; set; }
        public int NumNode { get; set; }
    }

    public struct BorderData
    {
        public BorderData(double h, double T_inf, double q)
        {
            this.h = h;
            this.T_inf = T_inf;
            this.q = q;
           
        }
        public double h { get; set; }
        public double T_inf { get; set; }
        public double q { get; set; }
   
    }
}
