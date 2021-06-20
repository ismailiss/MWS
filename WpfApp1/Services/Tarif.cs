using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MWSAPP.Services
{
    public static class Tarif
    {
        public static decimal CountIntervals(int[,] arr, decimal V, int N)
        {
            // Variable to store the count of intervals
            int count = 0;
            decimal tarif = 0;
            // Variables to store start and end of an interval
            int li, ri, i = 0;
            bool found = false;
            while(!found){
                li = arr[i, 0];
                ri = arr[i, 1];
                // Implies V lies in the interval
                // so increase count
                if (V >= li && V <= ri)
                    found = true;
                else
                    i++;
            }          
            switch (count)
            {
                case 0:
                    tarif = 10;
                    break;
                case 1:
                    tarif = 11;
                    break;
                case 2:
                    tarif = 15;
                    break;
                case 3:
                    tarif = 20;
                    break;
                case 4:
                    tarif = 28;
                    break;
                case 5:
                    tarif = 28;
                    break;
                case 6:
                    tarif = 28;
                    break;
                default:
                    tarif = 60;
                    break;
            }
            return tarif;
        }
    }
}
