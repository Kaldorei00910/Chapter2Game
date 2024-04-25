using System.Runtime.CompilerServices;
using System.Runtime.ExceptionServices;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace Chapter2Game
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //초기설정
            Player player = new Player();
            List<Item> invenList = new List<Item>();
            List<Item> allitemList = new List<Item>();
            List<Dungeon> DungeonList = new List<Dungeon>();

            int atkPlus = 0;
            int defPlus = 0;
            string fileName = Directory.GetCurrentDirectory() + "\\SaveData.txt";
            string strData;

            Item mu_shuePlate = new Item("무쇠갑옷      ", "방어력", 5, "", "무쇠로 만들어져 튼튼한 갑옷입니다           ", 1000);
            Item spartaSpear = new Item("스파르타의 창", "공격력", 12, "", "스파르타의 전사들이 사용했다는 전설의 창입니다.", 1200);
            Item oldSword = new Item("낡은 검       ", "공격력", 2, "", "쉽게 볼 수 있는 낡은 검 입니다.                  ", 600);
            Item workPlate = new Item("수련자 갑옷  ", "방어력", 8, "", "수련에 도움을 주는 갑옷입니다.                   ", 1000);
            Item bronzeAxe = new Item("청동 도끼    ", "공격력", 5, "", "어디선가 사용됐던거 같은 도끼입니다.              ", 1500);
            Item spartaPlate = new Item("스파르타의 갑옷", "방어력", 15, "", "스파르타의 전사들이 사용했다는 전설의 갑옷입니다.", 3500);
            Item stone = new Item("애완 돌       ", "공격력", 1, "", "땅에서 주운 당신의 동반자입니다                         ", 1);

            Dungeon level1 = new Dungeon(1, 1000, 5);
            Dungeon level2 = new Dungeon(2, 1700, 11);
            Dungeon level3 = new Dungeon(3, 2500, 17);

            DungeonList.Add(level1);
            DungeonList.Add(level2);
            DungeonList.Add(level3);

            allitemList.Add(stone);
            allitemList.Add(oldSword);
            allitemList.Add(bronzeAxe);
            allitemList.Add(spartaSpear);
            allitemList.Add(mu_shuePlate);
            allitemList.Add(workPlate);
            allitemList.Add(spartaPlate);

            invenList.Add(stone);


            while (true)
            {
                string answer = ShowMainScreen();//메인스크린 호출 및 값 받아오기

                atkPlus = 0;//장비로 인해 추가되는 공격력과 방어력 매번 메인스크린때마다 변경해주기
                defPlus = 0;
                foreach (Item item in invenList)
                {
                    if (item.doesEquip == "[E]")
                    {
                        if (item.type == "공격력")
                        {
                            atkPlus += item.value;
                        }
                        else
                        {
                            defPlus += item.value;
                        }
                    }
                }

                if (answer == "1")
                {
                    ShowStatus(player, atkPlus, defPlus);//상태보기 수행, 플레이어 객체와 추가 공/방 값을 가져가서 표시함
                }
                else if (answer == "2")
                {
                    ShowInventory(invenList);//인벤토리 수행
                }
                else if (answer == "3")
                {
                    ShowStore(player, invenList, allitemList);//상점 진입
                }
                else if (answer == "4")
                {
                    DungeonEnter(player, DungeonList, atkPlus, defPlus);//던전 진입

                }
                else if ( answer == "5")//휴식하기
                {
                    GoRest(player);

                }
                else if( answer == "6")//저장하기
                {
                    strData = Save(player, invenList, fileName);

                }
                else if( answer == "7")//불러오기
                {
                    invenList = Load(player, invenList, fileName);

                }
                else if (answer == "showmethemoney")//치트기능..돈증가
                {
                    player.gold += 1000000;
                }
                else
                {
                    Console.WriteLine("잘못된 입력입니다.");
                    //메인화면으로
                }
            }

        }//메인 종료(게임종료)

        private static List<Item> Load(Player player, List<Item> invenList, string fileName)
        {
            FileInfo fi = new FileInfo(fileName);
            if (fi.Exists)
            {
                FileStream saveData = new FileStream(fileName, FileMode.Open);
                StreamReader saveReader = new StreamReader(saveData, Encoding.UTF8);
                StringBuilder strBulider = new StringBuilder(1000);


                while (saveReader.Peek() > -1)
                {
                    string strLine = saveReader.ReadLine();
                    strBulider.AppendLine(strLine);
                }
                saveReader.Close();
                saveData.Close();

                string strTemp = strBulider.ToString();
                string[] data = strTemp.Split('\n');

                player.SetPlayer(data);// 플레이어의 데이터 불러온 값으로 변경

                using (FileStream fs = new FileStream("Itemlist.dat", FileMode.Open))//인벤토리 불러오기
                {
                    BinaryFormatter bf = new BinaryFormatter();
                    invenList = (List<Item>)bf.Deserialize(fs);
                }
                Console.WriteLine("불러오기 완료!(엔터를 눌러 확인..)");
                Console.ReadLine();
            }
            else
            {
                Console.WriteLine("저장된 파일이 없습니다!(엔터를 눌러 확인..)");
                Console.ReadLine();

            }

            return invenList;
        }

        private static string Save(Player player, List<Item> invenList, string fileName)
        {
            string strData;
            //File.Create("SaveData.dat");
            FileStream saveData = new FileStream(fileName, FileMode.Create);
            StreamWriter saveWriter = new StreamWriter(saveData, Encoding.UTF8);
            strData = $"{player.level}\n{player.attacklevel}\n{player.defencelevel}\n{player.hp}\n{player.gold}\n{player.clearCount}";
            saveWriter.Write(strData);

            saveWriter.Close();
            saveData.Close();

            using (FileStream fs = new FileStream("Itemlist.dat", FileMode.Create))//인벤토리 저장
            {
                BinaryFormatter bf = new BinaryFormatter();
                bf.Serialize(fs, invenList);
            }

            return strData;
        }

        private static void GoRest(Player player)
        {
            string answerRest = "";
            Console.Clear();
            Console.WriteLine("휴식하기");
            Console.WriteLine("500 G 를 내면 체력을 회복할 수 있습니다. (보유 골드 : {0} G)            ", player.gold);
            Console.WriteLine("\n1. 휴식하기");
            Console.WriteLine("0. 나가기");
            Console.WriteLine("\n원하시는 행동을 입력해주세요.");

            answerRest = Console.ReadLine();
            if (answerRest == "1")
            {
                if (player.gold > 500)
                {
                    Console.Clear();
                    Console.WriteLine("체력을 회복했습니다!");
                    Console.WriteLine("플레이어 체력 : {0} -> {1}       ", player.hp, 100);
                    Console.WriteLine("보유 Gold : {0} -> {1}       ", player.gold, player.gold - 500);
                    player.hp = 100;
                    player.gold -= 500;
                    Console.WriteLine("\n엔터를 눌러 계속...");
                    Console.ReadLine();

                }
                else
                {
                    Console.Clear();
                    Console.WriteLine("보유 Gold가 부족합니다!");
                    Console.WriteLine("\n엔터를 눌러 계속...");
                    Console.ReadLine();
                }
            }
        }

        private static void DungeonEnter(Player player, List<Dungeon> DungeonList, int atkPlus, int defPlus)
        {
            string answerDungeon = "";
            int justhp = 0;
            int justgold = 0;
            Random random = new Random();
            do
            {
                Console.Clear();
                Console.BackgroundColor = ConsoleColor.DarkRed;
                Console.WriteLine("[던전입장]");
                Console.ResetColor();
                Console.WriteLine("난이도에 맞는 던전을 선택해 주세요!");
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("\n1. 수퍼 겁쟁이들의 쉽터 \t | 방어력 5 이상 권장");
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine("2. 보통 겁쟁이들의 쉼터 \t | 방어력 11 이상 권장");
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("3. !!! 사나이 클럽 !!!   \t | 방어력 17 이상 권장\n");
                Console.ResetColor();
                Console.WriteLine("0. 겁에 질려 도망가기");
                Console.WriteLine("\n원하시는 행동을 입력해주세요.");

                answerDungeon = Console.ReadLine();

                if (int.TryParse(answerDungeon, out int stage))//숫자 입력받음
                {
                    if (stage >= 1 && stage < 4)//던전 입장, 0입력시 마을로감
                    {

                        if (DungeonList[stage - 1].needDef > player.defencelevel + defPlus)//던전의 방어도가 더 높을경우(40퍼확률 실패)
                        {
                            if (random.Next(1, 10) <= 4)//확률적 실패한 경우
                            {
                                if (player.hp <= 10)//체력이 10이하로 던전 들어가 실패 경우 : 사망
                                {
                                    Console.Clear();
                                    Console.ForegroundColor = ConsoleColor.DarkRed;
                                    Console.WriteLine("\n    -    더 많은 방어도를 쌓고 와야했었는데...    -\n\n");
                                    GameOver();
                                }
                                Console.Clear();
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine("던전에 실패했습니다.\n");
                                Console.WriteLine("[탐험 결과]\n체력이 절반 감소했습니다....");
                                Console.ResetColor();
                                Console.WriteLine("체력 {0} -> {1}", player.hp, player.hp / 2);
                                player.hp = (int)((float)player.hp / 2.0f);

                                Console.WriteLine("\n엔터를 눌러 계속...");
                                Console.ReadLine();
                            }
                            else
                            {
                                Console.Clear();
                                Console.BackgroundColor = ConsoleColor.DarkRed;
                                Console.WriteLine("던전에 실패했습니다.\n");
                                Console.WriteLine("[탐험 결과]\n목숨만 챙겨서 도망쳤습니다..");
                                Console.ResetColor();
                                Console.WriteLine("\n엔터를 눌러 계속...");
                                Console.ReadLine();
                            }
                        }
                        else
                        {
                            if (player.hp == 1)//체력이 1인채로 던전 들어간 경우 : 사망
                            {
                                Console.Clear();
                                Console.ForegroundColor = ConsoleColor.DarkRed;
                                Console.WriteLine("\n    -    체력 좀 채우고 올껄..    -\n\n");
                                GameOver();
                            }
                            player.clearCount++;
                            Console.Clear();
                            Console.ForegroundColor = ConsoleColor.Black;
                            Console.BackgroundColor = ConsoleColor.DarkGreen;
                            Console.WriteLine("던전 클리어.");
                            Console.WriteLine("축하합니다!!");
                            Console.WriteLine("{0}단계 던전을 클리어 하였습니다\n", DungeonList[stage - 1].dlevel);
                            Console.ResetColor();
                            Console.WriteLine("[탐험 결과]");

                            justhp = player.hp;
                            player.hp -= random.Next(20 + (DungeonList[stage - 1].needDef - player.defencelevel - defPlus), 35 + (DungeonList[stage - 1].needDef - player.defencelevel - defPlus));
                            if(player.hp < 1)
                            { 
                                player.hp = 1; 
                            }
                            Console.WriteLine("체력 {0} -> {1}", justhp, player.hp);

                            justgold = player.gold;
                            player.gold += (int)((float)DungeonList[stage - 1].reward * (1.0f + random.Next((int)(player.attacklevel + (float)atkPlus), (int)((player.attacklevel + (float)atkPlus) * 2.0f))));
                            Console.WriteLine("Gold {0} -> {1}", justgold, player.gold);

                            if(player.clearCount == player.level)
                            {
                                player.clearCount = 0;
                                Console.WriteLine("\nLEVEL UP!");
                                Console.WriteLine("플레이어 레벨 : {0} -> {1}     ",player.level,player.level+1);
                                player.level += 1;
                                Console.WriteLine("플레이어 공격력 : {0} -> {1}",player.attacklevel, player.attacklevel+0.5f);
                                Console.WriteLine("플레이어 방어력 : {0} -> {1}", player.defencelevel, player.defencelevel + 1);
                                player.attacklevel += 0.5f;
                                player.defencelevel += 1;
                            }
                            Console.WriteLine("\n엔터를 눌러 계속...");
                            Console.ReadLine();
                        }
                        if (player.hp <= 0)
                        {
                            GameOver();
                        }
                    }
                    else
                    {
                        Console.WriteLine("올바른 번호를 입력하세요");
                    }
                }
                else
                {
                    Console.WriteLine("올바른 숫자를 입력하세요");
                }

            }
            while (answerDungeon != "0");
        }

        private static void GameOver()
        {
            
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine("\r\n                                          \r\n                                      ,,  \r\n`7MM\"\"\"Yb.                          `7MM  \r\n  MM    `Yb.                          MM  \r\n  MM     `Mb  .gP\"Ya   ,6\"Yb.    ,M\"\"bMM  \r\n  MM      MM ,M'   Yb 8)   MM  ,AP    MM  \r\n  MM     ,MP 8M\"\"\"\"\"\"  ,pm9MM  8MI    MM  \r\n  MM    ,dP' YM.    , 8M   MM  `Mb    MM  \r\n.JMMmmmdP'    `Mbmmd' `Moo9^Yo. `Wbmd\"MML.\r\n                                          \r\n                                          \r\n");
            Console.WriteLine("\n\n\n\n  - 당신은 죽었습니다. ");
            Console.WriteLine("게임이 종료됩니다..");
            Console.ReadLine();
            Environment.Exit(0);
        }

        private static void ShowStore(Player player, List<Item> invenList, List<Item> allitemList)
        {
            string storeAnswer = "firstopen";

            while (storeAnswer != "0")
            {
                int storeCount = 0;
                Console.Clear();
                Console.BackgroundColor = ConsoleColor.DarkRed;
                Console.WriteLine("[상점]");
                Console.ResetColor();
                Console.WriteLine("필요한 아이템을 얻을 수 있는 상점입니다.\n");
                Console.WriteLine("[보유 골드]");
                Console.WriteLine("{0} G\n", player.gold);
                Console.WriteLine("[아이템 목록]");
                foreach (Item exitem in allitemList)
                {
                    storeCount++;//현재 아이템 갯수(숫자)만큼 저장됨
                    if (invenList.Contains(exitem))
                    {
                        Console.WriteLine(" - {0}. {1}\t| {2} +{3} | {4}\t| 구매완료", storeCount, exitem.name, exitem.type, exitem.value, exitem.story);
                    }
                    else
                    {
                        Console.WriteLine(" - {0}. {1}\t| {2} +{3} | {4}\t| {5} G", storeCount, exitem.name, exitem.type, exitem.value, exitem.story, exitem.cost);

                    }
                }        
                if (storeAnswer == "1")//구매 시작
                {
                    BuyAtStore(player, invenList, allitemList, storeCount);
                }
                else if(storeAnswer == "2")//판매 시작
                {
                    SellAtStore(player, invenList, allitemList, storeCount);
                }
                Console.SetCursorPosition(0, 8 + storeCount);
                Console.Write("                                           \r");
                Console.WriteLine("1.아이템 구매");
                Console.WriteLine("2.아이템 판매");
                Console.Write("                                           \r");
                Console.WriteLine("0.나가기\n   ");
                Console.WriteLine("원하시는 행동을 입력해주세요");
                Console.Write("                                           \r");


                storeAnswer = Console.ReadLine();

            }
        }

        private static void SellAtStore(Player player, List<Item> invenList, List<Item> allitemList, int storeCount)
        {
            string payAnswer = "firstopen";
            while (payAnswer != "0")
            {
                Console.SetCursorPosition(0, 7 + storeCount);
                Console.WriteLine("\n판매할 아이템을 선택해주세요.");
                Console.WriteLine("\n0. 상점으로 돌아가기\n\n");

                if (int.TryParse(payAnswer, out int number))
                {
                    if (number <= 0 || number > storeCount)
                    {
                        Console.WriteLine("올바른 값을 입력하세요");
                    }
                    else
                    {
                        if (invenList.Contains(allitemList[number - 1])!) //아이템을 가지고 있다면
                        {
                            invenList.Remove(allitemList[number - 1]);
                            player.gold += (int)((float)(allitemList[number - 1].cost) * 0.85f);
                            Console.SetCursorPosition(0, 4);
                            Console.Write("\r");
                            Console.WriteLine("{0} G", player.gold);
                            Console.SetCursorPosition(0, 7 + number - 1);
                            Console.WriteLine(" - {0}. {1}\t| {2} +{3} | {4}\t| {5} G       ", number, allitemList[number - 1].name, allitemList[number - 1].type, allitemList[number - 1].value, allitemList[number - 1].story, allitemList[number - 1].cost);

                        }
                        else
                        {
                            Console.SetCursorPosition(0, 7 + storeCount);
                            Console.WriteLine("\n소유하고 있지 않은 아이템입니다.\n판매할 아이템을 선택해주세요.");
                        }
                    }
                }
                else if (payAnswer == "firstopen")
                {

                }
                else
                {
                    Console.WriteLine("숫자를 입력하세요");
                }
                Console.SetCursorPosition(0, 11 + storeCount);
                payAnswer = Console.ReadLine();
            }
        }

        private static void BuyAtStore(Player player, List<Item> invenList, List<Item> allitemList, int storeCount)
        {
            string buyAnswer = "firstopen";
            while (buyAnswer != "0")
            {
                Console.SetCursorPosition(0, 7 + storeCount);
                Console.WriteLine("\n구매할 아이템을 선택해주세요.");
                Console.WriteLine("\n0. 상점으로 돌아가기\n\n");

                if (int.TryParse(buyAnswer, out int number))
                {
                    if (number <= 0 || number > storeCount)
                    {
                        Console.WriteLine("올바른 값을 입력하세요");
                    }
                    else
                    {
                        if (invenList.Contains(allitemList[number - 1])!) //아이템을 가지고 있다면
                        {
                            Console.WriteLine("이미 구매한 아이템입니다.");
                        }
                        else if (player.gold < allitemList[number - 1].cost) //금액 확인
                        {//금액 확인 후 결제과정
                            Console.WriteLine("Gold가 부족합니다.");
                        }
                        else
                        {
                            invenList.Add(allitemList[number - 1]);
                            player.gold = player.gold - allitemList[number - 1].cost;
                            Console.SetCursorPosition(0, 4);
                            Console.Write("\r");
                            Console.WriteLine("{0} G", player.gold);
                            Console.SetCursorPosition(0, 7 + number - 1);
                            Console.WriteLine(" - {0}. {1}\t| {2} +{3} | {4}\t| 구매완료", number, allitemList[number - 1].name, allitemList[number - 1].type, allitemList[number - 1].value, allitemList[number - 1].story);
                        }
                    }
                }
                else if (buyAnswer == "firstopen")
                {

                }
                else
                {
                    Console.WriteLine("숫자를 입력하세요");
                }
                Console.SetCursorPosition(0, 11 + storeCount);
                buyAnswer = Console.ReadLine();
            }
        }

        private static void ShowStatus(Player player, int atkPlus_, int defPlus_)//상태보기 수행
        {
            int atkPlus = atkPlus_;
            int defPlus = defPlus_;

            Console.Clear();
            Console.BackgroundColor = ConsoleColor.DarkRed;
            Console.WriteLine("[상태 보기]");
            Console.ResetColor();
            Console.WriteLine("캐릭터의 정보가 표시됩니다\n");
            Console.WriteLine("Lv : {0}", player.level);
            Console.WriteLine("Chad : {0}", player.chad);
            Console.WriteLine("공격력 : {0} (+{1})", player.attacklevel, atkPlus);
            Console.WriteLine("방어력 : {0} (+{1})", player.defencelevel, defPlus);
            Console.WriteLine("체  력 : {0}", player.hp);
            Console.WriteLine("Gold : {0}\n", player.gold);
            Console.WriteLine("0. 나가기\n");
            Console.WriteLine("원하시는 행동을 입력해주세요");
            while (Console.ReadLine() != "0")
            {
                Console.WriteLine("올바른 값을 입력하세요");
            }
            Console.Clear();

        }

        private static void ShowInventory(List<Item> invenList)//인벤토리 수행
        {
            int count = 0;

            Console.Clear();
            Console.BackgroundColor = ConsoleColor.DarkRed;
            Console.WriteLine("[인벤토리]");
            Console.ResetColor();
            Console.WriteLine("보유 중인 아이템을 관리할 수 있습니다.\n");
            Console.WriteLine("[아이템 목록]\n");

            count = ShowItemList(invenList, count);

            Console.WriteLine("\n1. 장착 관리");
            Console.WriteLine("0. 나가기\n");
            Console.WriteLine("원하시는 행동을 입력해주세요");

            string answer = Console.ReadLine();

            while (answer != "0" && answer != "1")
            {
                Console.WriteLine("올바른 값을 입력하세요");
                answer = Console.ReadLine();
            }

            if (answer == "1")
            {
                ChangeEquipment(count, invenList);
            }

            Console.Clear();
        }

        private static int ShowItemList(List<Item> invenList, int count)
        {
            count = 0;
            Console.SetCursorPosition(0, 5);
            foreach (Item exitem in invenList)
            {
                Console.WriteLine(" - {0}{1}\t| {2} +{3} | {4}", exitem.doesEquip, exitem.name, exitem.type, exitem.value, exitem.story);
                count++;
            }

            return count;
        }

        private static void ChangeEquipment(int count, List<Item> invenList)
        {
            string answer = "";
            do
            {
                answer = "";
                for (int i = count; i > 0; i--)
                {
                    Console.SetCursorPosition(0, 4 + i);
                    Console.WriteLine("{0}.", i);
                }
                Console.SetCursorPosition(0, 9 + count);
                Console.WriteLine("착용(해제)하려는 장비의 번호를 입력해주세요(취소는 0번)");
                answer = Console.ReadLine();
                if (int.TryParse(answer, out int number))
                {
                    if (number <= 0 || number > count)
                    {
                        Console.WriteLine("올바른 값을 입력하세요");
                    }
                    else
                    {
                        if (invenList[number - 1].doesEquip == "[E]")
                        {
                            invenList[number - 1].doesEquip = "";
                        }
                        else//아이템을 장착하고, 같은 부류의 아이템 장착 해제하기!
                        {
                            invenList[number - 1].doesEquip = "[E]";

                            int searchEquipSame = invenList.FindIndex(what => what.doesEquip.Equals("[E]") && !what.Equals(invenList[number-1]) && what.type.Equals(invenList[number-1].type));

                            if (searchEquipSame != -1)//같은 부류 아이템 찾은경우
                            {
                                invenList[searchEquipSame].doesEquip = "";//해당 아이템 해제시키기
                            }
                        }

                    }

                }
                else
                {
                    Console.WriteLine("숫자를 입력하세요");
                }
                ShowItemList(invenList, count);
            }
            while (answer != "0");
        }

        private static string ShowMainScreen()
        {
            string answer = "a";
            Console.Clear();
            Console.BackgroundColor = ConsoleColor.DarkRed;
            Console.WriteLine("[Title Screen]");
            Console.ResetColor();
            Console.WriteLine("스파르타 마을에 오신 여러분 환영합니다.");
            Console.WriteLine("이곳에서 던전으로 들어가기전 활동을 할 수 있습니다");
            Console.WriteLine("\n1. 상태 보기");
            Console.WriteLine("2. 인벤토리");
            Console.WriteLine("3. 상점");
            Console.WriteLine("4. 던전 입장!");
            Console.WriteLine("5. 쉬러가기\n");
            Console.WriteLine("6. 저장하기");
            Console.WriteLine("7. 불러오기");


            Console.WriteLine("\n원하시는 행동을 입력해주세요.");
            answer = Console.ReadLine();
            return answer;
        }


        class Player//플레이어에 대한 정보 기입
        {
            public int level = 1;
            public string chad = "전사";
            public float attacklevel = 10;
            public int defencelevel = 5;
            public int hp = 100;
            public int gold = 1500;
            public int clearCount = 0;

            public void SetPlayer(string[] loadData)
            {
                level = int.Parse(loadData[(int)PlayerData.level]);
                attacklevel = float.Parse(loadData[(int)PlayerData.attacklevel]);
                defencelevel = int.Parse(loadData[(int)PlayerData.defencelevel]);
                hp = int.Parse(loadData[(int)PlayerData.hp]);
                gold = int.Parse(loadData[(int)PlayerData.gold]);
                clearCount = int.Parse(loadData[(int)PlayerData.clearCount]);

            }
        }
        [Serializable]
        class Item//아이템에 대한 정보
        {
            public string name;
            public string type;
            public int value;
            public string doesEquip;
            public bool doesHave;
            public string story;
            public int cost;


            public Item(string name_, string type_, int value_, string doesEquip_, string story_, int cost_)
            {
                name = name_;
                type = type_;
                value = value_;
                doesEquip = doesEquip_;
                story = story_;
                cost = cost_;
            }
        }

        class Dungeon//던전에 대한 정보
        {
            public int dlevel;
            public int reward;
            public int needDef;


            public Dungeon(int dlevel_, int reward_, int needDef_)
            {
                dlevel = dlevel_;
                reward = reward_;
                needDef = needDef_;

            }
        }

        enum PlayerData
        {
            level,
            attacklevel,
            defencelevel,
            hp,
            gold,
            clearCount
        }

    }
}
