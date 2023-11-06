using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Currency
{
        public static string DisplayCurrency(ulong money)
        {
                int moneyLength = money.ToString().Length;
                char firstChar = money.ToString()[0];
                char secondChar = money.ToString()[1];
                char thirdChar = money.ToString()[2];

                switch (moneyLength)
                {
                        case 2:
                                if (money == 0)
                                {
                                        return "$0.00";
                                }
                                break;
                        case 3:
                                if (money > 0 && money < 1000)
                                {
                                        return money.ToString("C");
                                }
                                break;
                        case 4:
                                return $"{firstChar}K";

                        case 5:
                                return $"{firstChar}{secondChar}K";

                        case 6:
                                return $"{firstChar}{secondChar}{thirdChar}K";

                        case 7:
                                return $"{firstChar}.{secondChar}{thirdChar}M";

                        case 8:
                                if (money >= 10000000 && money <= 99999999)
                                {
                                        return $"{firstChar}{secondChar}.{thirdChar}M";
                                }
                                break;

                        case 9:
                                if (money >= 100000000 && money <= 999999999)
                                {
                                        return $"{firstChar}{secondChar}{thirdChar}M";
                                }
                                break;

                        // 1.00B - 9.99B
                        case 10:
                                if (money >= 1000000000 && money <= 9999999999)
                                {
                                        return $"{firstChar}.{secondChar}{thirdChar}B";
                                }
                                break;

                        // 10.0B - 99.9B
                        case 11:
                                if (money >= 10000000000 && money <= 99999999999)
                                {
                                        return $"{firstChar}{secondChar}.{thirdChar}B";
                                }
                                break;

                        // 100B - 999B
                        case 12:
                                if (money >= 100000000000 && money <= 999999999999)
                                {
                                        return $"{firstChar}{secondChar}{thirdChar}B";
                                }
                                break;

                        // 1.00T - 9.99T
                        case 13:
                                if (money >= 1000000000000 & money <= 9999999999999)
                                {
                                        return $"{firstChar}.{secondChar}{thirdChar}T";
                                }
                                break;

                        // 10.0T - 99.9T
                        case 14:
                                if (money >= 10000000000000 && money <= 99999999999999)
                                {
                                        return $"{firstChar}{secondChar}.{thirdChar}T";
                                }
                                break;

                        // 100T - 999T
                        case 15:
                                if (money >= 100000000000000 && money <= 999999999999999)
                                {
                                        return $"{firstChar}{secondChar}{thirdChar}T";
                                }
                                break;

                        // 1.00Q - 9.99Q
                        case 16:
                                if (money >= 1000000000000000 && money <= 9999999999999999)
                                {
                                        return $"{firstChar}.{secondChar}{thirdChar}Q";
                                }
                                break;

                        // 10.0Q - 99.9Q
                        case 17:
                                if (money >= 10000000000000000 && money <= 99000000000000000)
                                {
                                        return $"{firstChar}{secondChar}.{thirdChar}Q";
                                }
                                break;

                        // 100Q - 999Q
                        case 18:
                                if (money >= 100000000000000000 && money <= 999999999999999999)
                                {
                                        return $"{firstChar}{secondChar}{thirdChar}Q";
                                }
                                break;

                        // 1.00S - 9.99S
                        case 19:
                                if (money >= 1000000000000000000 && money <= 9999999999999999999)
                                {
                                        return $"{firstChar}.{secondChar}{thirdChar}S";
                                }
                                break;
                }
                return string.Empty;
        }
}
