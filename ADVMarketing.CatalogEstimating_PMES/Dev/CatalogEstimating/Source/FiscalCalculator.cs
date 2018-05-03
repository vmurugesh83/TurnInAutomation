using System;
using System.Collections.Generic;
using System.Text;

namespace CatalogEstimating
{
    public static class FiscalCalculator
    {
        public static int DaysApart(DateTime StartDate, DateTime EndDate)
        {
            TimeSpan TS = new TimeSpan(EndDate.Ticks - StartDate.Ticks);
            return TS.Days;
        }

        public static int FiscalYear(DateTime inDate)
        {
            DateTime curr_mo_start_dt = new DateTime(1993, 1, 31);
            int curr_fiscal_yr = 1992;
            int curr_fiscal_mth = 1;
            int curr_mth_nbr_of_weeks = 5;
            int next_mth_nbr_of_weeks = 4;
            bool found = false;

            while (!found)
            {
                curr_fiscal_mth = 1;
                ++curr_fiscal_yr;

                while (curr_fiscal_mth < 13 && !found)
                {
                    // Account for 53 week years
                    // Here we supply the first day of the last fiscal month of the 53 week year.
                    if (FiscalCalculator.DaysApart(curr_mo_start_dt, new DateTime(1995, 12, 31)) == 0 ||
                            FiscalCalculator.DaysApart(curr_mo_start_dt, new DateTime(2000, 12, 31)) == 0 ||
                            FiscalCalculator.DaysApart(curr_mo_start_dt, new DateTime(2006, 12, 31)) == 0 ||
                            FiscalCalculator.DaysApart(curr_mo_start_dt, new DateTime(2012, 12, 30)) == 0 ||
                            FiscalCalculator.DaysApart(curr_mo_start_dt, new DateTime(2017, 12, 31)) == 0)
                        curr_mth_nbr_of_weeks = 6;

                    if (curr_mth_nbr_of_weeks == 4 && next_mth_nbr_of_weeks == 4)
                    {
                        curr_mo_start_dt = curr_mo_start_dt.AddDays(35);
                        next_mth_nbr_of_weeks = 5;
                    }
                    else if (curr_mth_nbr_of_weeks == 4 && next_mth_nbr_of_weeks == 5)
                    {
                        curr_mo_start_dt = curr_mo_start_dt.AddDays(28);
                        curr_mth_nbr_of_weeks = 5;
                        next_mth_nbr_of_weeks = 4;
                    }
                    else if (curr_mth_nbr_of_weeks == 5 && next_mth_nbr_of_weeks == 4)
                    {
                        curr_mo_start_dt = curr_mo_start_dt.AddDays(28);
                        curr_mth_nbr_of_weeks = 4;
                    }
                    else if (curr_mth_nbr_of_weeks == 6 && next_mth_nbr_of_weeks == 5)
                    {
                        curr_mo_start_dt = curr_mo_start_dt.AddDays(35);
                        curr_mth_nbr_of_weeks = 5;
                        next_mth_nbr_of_weeks = 4;
                    }

                    if (FiscalCalculator.DaysApart(inDate, curr_mo_start_dt) > 0)
                        found = true;
                    else
                        ++curr_fiscal_mth;
                }
            }

            return curr_fiscal_yr;
        }

        public static string MonthName(int fiscalMonth)
        {
            switch (fiscalMonth)
            {
                case 1:
                    return "February";
                case 2:
                    return "March";
                case 3:
                    return "April";
                case 4:
                    return "May";
                case 5:
                    return "June";
                case 6:
                    return "July";
                case 7:
                    return "August";
                case 8:
                    return "September";
                case 9:
                    return "October";
                case 10:
                    return "November";
                case 11:
                    return "December";
                default:
                    return "January";
            }
        }

        public static int FiscalMonth(DateTime inDate)
        {
            DateTime curr_mo_start_dt = new DateTime(1993, 1, 31);
            int curr_fiscal_yr = 1992;
            int curr_fiscal_mth = 1;
            int curr_mth_nbr_of_weeks = 5;
            int next_mth_nbr_of_weeks = 4;
            bool found = false;

            while (!found)
            {
                curr_fiscal_mth = 1;
                ++curr_fiscal_yr;

                while (curr_fiscal_mth < 13 && !found)
                {
                    // Account for 53 week years
                    // Here we supply the first day of the last fiscal month of the 53 week year.
                    if (FiscalCalculator.DaysApart(curr_mo_start_dt, new DateTime(1995, 12, 31)) == 0 ||
                            FiscalCalculator.DaysApart(curr_mo_start_dt, new DateTime(2000, 12, 31)) == 0 ||
                            FiscalCalculator.DaysApart(curr_mo_start_dt, new DateTime(2006, 12, 31)) == 0 ||
                            FiscalCalculator.DaysApart(curr_mo_start_dt, new DateTime(2012, 12, 30)) == 0 ||
                            FiscalCalculator.DaysApart(curr_mo_start_dt, new DateTime(2017, 12, 31)) == 0)
                        curr_mth_nbr_of_weeks = 6;

                    if (curr_mth_nbr_of_weeks == 4 && next_mth_nbr_of_weeks == 4)
                    {
                        curr_mo_start_dt = curr_mo_start_dt.AddDays(35);
                        next_mth_nbr_of_weeks = 5;
                    }
                    else if (curr_mth_nbr_of_weeks == 4 && next_mth_nbr_of_weeks == 5)
                    {
                        curr_mo_start_dt = curr_mo_start_dt.AddDays(28);
                        curr_mth_nbr_of_weeks = 5;
                        next_mth_nbr_of_weeks = 4;
                    }
                    else if (curr_mth_nbr_of_weeks == 5 && next_mth_nbr_of_weeks == 4)
                    {
                        curr_mo_start_dt = curr_mo_start_dt.AddDays(28);
                        curr_mth_nbr_of_weeks = 4;
                    }
                    else if (curr_mth_nbr_of_weeks == 6 && next_mth_nbr_of_weeks == 5)
                    {
                        curr_mo_start_dt = curr_mo_start_dt.AddDays(35);
                        curr_mth_nbr_of_weeks = 5;
                        next_mth_nbr_of_weeks = 4;
                    }

                    if (FiscalCalculator.DaysApart(inDate, curr_mo_start_dt) > 0)
                        found = true;
                    else
                        ++curr_fiscal_mth;
                }
            }

            return curr_fiscal_mth;
        }

        /// <summary>
        /// Calculates the Season of the inDate.
        /// 1 - Spring
        /// 2 - Fall
        /// </summary>
        /// <param name="inDate"></param>
        /// <returns></returns>
        public static int SeasonID(DateTime inDate)
        {
            DateTime curr_mo_start_dt = new DateTime(1993, 1, 31);
            int curr_fiscal_yr = 1992;
            int curr_fiscal_mth = 1;
            int curr_mth_nbr_of_weeks = 5;
            int next_mth_nbr_of_weeks = 4;
            bool found = false;

            while (!found)
            {
                curr_fiscal_mth = 1;
                ++curr_fiscal_yr;

                while (curr_fiscal_mth < 13 && !found)
                {
                    // Account for 53 week years
                    // Here we supply the first day of the last fiscal month of the 53 week year.
                    if (FiscalCalculator.DaysApart(curr_mo_start_dt, new DateTime(1995, 12, 31)) == 0 ||
                            FiscalCalculator.DaysApart(curr_mo_start_dt, new DateTime(2000, 12, 31)) == 0 ||
                            FiscalCalculator.DaysApart(curr_mo_start_dt, new DateTime(2006, 12, 31)) == 0 ||
                            FiscalCalculator.DaysApart(curr_mo_start_dt, new DateTime(2012, 12, 30)) == 0 ||
                            FiscalCalculator.DaysApart(curr_mo_start_dt, new DateTime(2017, 12, 31)) == 0)
                        curr_mth_nbr_of_weeks = 6;

                    if (curr_mth_nbr_of_weeks == 4 && next_mth_nbr_of_weeks == 4)
                    {
                        curr_mo_start_dt = curr_mo_start_dt.AddDays(35);
                        next_mth_nbr_of_weeks = 5;
                    }
                    else if (curr_mth_nbr_of_weeks == 4 && next_mth_nbr_of_weeks == 5)
                    {
                        curr_mo_start_dt = curr_mo_start_dt.AddDays(28);
                        curr_mth_nbr_of_weeks = 5;
                        next_mth_nbr_of_weeks = 4;
                    }
                    else if (curr_mth_nbr_of_weeks == 5 && next_mth_nbr_of_weeks == 4)
                    {
                        curr_mo_start_dt = curr_mo_start_dt.AddDays(28);
                        curr_mth_nbr_of_weeks = 4;
                    }
                    else if (curr_mth_nbr_of_weeks == 6 && next_mth_nbr_of_weeks == 5)
                    {
                        curr_mo_start_dt = curr_mo_start_dt.AddDays(35);
                        curr_mth_nbr_of_weeks = 5;
                        next_mth_nbr_of_weeks = 4;
                    }

                    if (FiscalCalculator.DaysApart(inDate, curr_mo_start_dt) > 0)
                        found = true;
                    else
                        ++curr_fiscal_mth;
                }
            }

            if (curr_fiscal_mth < 7)
                return 1;
            else
                return 2;
        }
    }

}
