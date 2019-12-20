using System;
using System.Collections.Generic;
using System.Text;

namespace DataStructure.MultiDimentionalArray
{
    public class D2Array
    {
        
        int[,] seats;
        const int MAX_ROW = 25, MAX_COL = 3;

        public D2Array()
        {
            seats = new int[MAX_ROW, MAX_COL] 
            {
                { 65, 1, 0 },
                { 65, 2, 0 },
                { 65, 3, 1 },
                { 65, 4, 0 },
                { 65, 5, 0 },

                { 66, 1, 1 },
                { 66, 2, 1 },
                { 66, 3, 0 },
                { 66, 4, 0 },
                { 66, 5, 0 },
                
                { 67, 1, 1 },
                { 67, 2, 1 },
                { 67, 3, 1 },
                { 67, 4, 0 },
                { 67, 5, 0 },
                
                { 68, 1, 0 },
                { 68, 2, 0 },
                { 68, 3, 0 },
                { 68, 4, 0 },
                { 68, 5, 0 },

                { 69, 1, 0 },
                { 69, 2, 0 },
                { 69, 3, 0 },
                { 69, 4, 0 },
                { 69, 5, 0 }
            };

        }

        List<string> findConsecutiveSeats(int requestedSeatCount)
        {
            int seatCount = 0, prevRowNo = 1, row = 0, noOfSeatsPerRow = 5, noOfItemAddedInCurrentRowSeq = 0;
            const int ROW_SEQ = 0, SEAT_NO = 1, AVAILABILITY = 2;
            List<string> availableSeats = new List<string>();
            while (row < MAX_ROW)
            {
                if (seatCount == noOfSeatsPerRow)
                {
                    if (availableSeats.Count < requestedSeatCount)
                        availableSeats.RemoveAll(i => 1 == 1);
                    seatCount = 0;
                    prevRowNo = 0;
                    noOfItemAddedInCurrentRowSeq = 0;
                }

                bool isAvailable = !Convert.ToBoolean(seats[row, AVAILABILITY]);
                int currRowSeq = seats[row, ROW_SEQ], currRowNo = seats[row, SEAT_NO];
                
                if (isAvailable)
                {
                    if (availableSeats.Count == 0)
                    {
                        var seat = string.Format("{0}{1}", ((char)currRowSeq), currRowNo.ToString());
                        availableSeats.Add(seat);
                        prevRowNo = currRowNo;
                        noOfItemAddedInCurrentRowSeq++;
                    }
                    else
                    {
                        if (currRowNo == prevRowNo + 1 && noOfItemAddedInCurrentRowSeq < requestedSeatCount)
                        {
                            var seat = string.Format("{0}{1}", ((char)currRowSeq), currRowNo.ToString());
                            availableSeats.Add(seat);
                            prevRowNo = currRowNo;
                            noOfItemAddedInCurrentRowSeq++;
                        }
                    }
                }
                row++;
                seatCount++;
            }
            


            //for (int x = 0; x < 20; x++)
            //{
            //    int currRowSeq = seats[x, 0], currRowNo = seats[x, 1];
            //    bool isAvailable = !Convert.ToBoolean(seats[x, 2]);
            //    if (currRowSeq == prevRowSeq && currRowNo+1 == prevRowNo && isAvailable)
            //    {
            //        counter++;
            //        var seat = string.Format("{0}{1}", ((char)currRowSeq), seats[x, 1].ToString());
            //        availableSeats.Add(seat);
            //        if (counter == k)
            //            counter = 0;
            //    }
            //    else
            //    {
            //        counter = 0;
            //        prevRowSeq = seats[x, 0];                    
            //    }
            //}

            return availableSeats;
        }

        public static void ArrayMain()
        {
            D2Array array = new D2Array();
            var availableSeats = array.findConsecutiveSeats(3);
            for (int x = 0; x < availableSeats.Count; x++)
            {
                Console.Write("{0} ",availableSeats[x]);
            }
        }
    }
}
