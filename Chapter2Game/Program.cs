using System.Security.Cryptography.X509Certificates;

namespace Chapter2Game
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //초기설정
            Player player = new Player();
            List<Item> invenList = new List<Item>();
            int atkPlus = 0;
            int defPlus = 0;

            Item mu_shuePlate = new Item("무쇠갑옷    ", "방어력", 5, "[E]", "무쇠로 만들어져 튼튼한 갑옷입니다");
            Item spartaSpear = new Item("스파르타의 창", "공격력", 2, "", "스파르타의 전사들이 사용했다는 전설의 창입니다.");
            Item oldSword = new Item("낡은 검       ", "공격력", 2, "", "쉽게 볼 수 있는 낡은 검 입니다.");

            invenList.Add(mu_shuePlate);
            invenList.Add(oldSword);
            invenList.Add(spartaSpear);

            while (true)
            {
                char answer = ShowMainScreen();//메인스크린 호출 및 값 받아오기

                atkPlus = 0;//추가되는 공격력과 방어력 매번 메인스크린때마다 변경해주기
                defPlus = 0;
                foreach (Item item in invenList)
                {
                    if (item.doesEquip == "[E]")
                    {
                        if (item.type == "공격력")
                        {
                            atkPlus = atkPlus + item.value;
                        }
                        else
                        {
                            defPlus = defPlus + item.value;
                        }
                    }
                }

                if (answer == '1')
                {
                    ShowStatus(player,atkPlus,defPlus);//상태보기 수행, 플레이어 객체와 추가 공/방 값을 가져가서 표시함
                }
                else if (answer == '2')
                {
                    ShowInventory(invenList);//인벤토리 수행
                }
                else if (answer == '3')
                {
                    Console.WriteLine("상점 실행");
                    //상점
                }
                else
                {
                    Console.WriteLine("잘못된 입력입니다.");
                    //메인화면으로
                }
            }

        }//메인 종료(게임종료)


        private static void ShowStatus(Player player,int atkPlus_,int defPlus_)//상태보기 수행
        {
            int atkPlus = atkPlus_;
            int defPlus = defPlus_;

            Console.Clear();
            Console.WriteLine("상태 보기\n캐릭터의 정보가 표시됩니다\n");
            Console.WriteLine("Lv : {0}", player.level);
            Console.WriteLine("Chad : {0}", player.chad);
            Console.WriteLine("공격력 : {0} (+{1})", player.attacklevel,atkPlus);
            Console.WriteLine("방어력 : {0} (+{1})", player.defencelevel, defPlus);
            Console.WriteLine("체  력 : {0}", player.hp);
            Console.WriteLine("Gold : {0}\n", player.gold);
            Console.WriteLine("0. 나가기\n");
            Console.WriteLine("원하시는 행동을 입력해주세요");
            while (char.Parse(Console.ReadLine()) != '0')
            {
                Console.WriteLine("올바른 값을 입력하세요");
            }
            Console.Clear();

        }

        private static void ShowInventory(List<Item> invenList)//인벤토리 수행
        {
            int count = 0;

            Console.Clear();
            Console.WriteLine("인벤토리\n보유 중인 아이템을 관리할 수 있습니다.\n");
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
                        if (invenList[number-1].doesEquip == "[E]")
                        {
                            invenList[number - 1].doesEquip = "";
                        }
                        else
                        {
                            invenList[number - 1].doesEquip = "[E]";
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

        private static char ShowMainScreen()
        {
            char answer = 'a';
            Console.WriteLine("스파르타 마을에 오신 여러분 환영합니다.");
            Console.WriteLine("이곳에서 던전으로 들어가기전 활동을 할 수 있습니다");
            Console.WriteLine("\n1. 상태 보기");
            Console.WriteLine("2. 인벤토리");
            Console.WriteLine("3. 상점");
            Console.WriteLine("\n원하시는 행동을 입력해주세요.");
            answer = Char.Parse(Console.ReadLine());
            return answer;
        }
    
    
        class Player//플레이어에 대한 정보 기입
        {
            public int level = 1;
            public string chad = "전사";
            public int attacklevel = 10;
            public int defencelevel = 5;
            public int hp = 100;
            public int gold = 1500;
        }

        class Item
        {
            public string name;
            public string type;
            public int value;
            public string doesEquip;
            public bool doesHave;
            public string story;


            public Item(string name_, string type_, int value_, string doesEquip_, string story_)
            {
                name = name_;
                type = type_;
                value = value_;
                doesEquip = doesEquip_;
                story = story_;

            }

        }

    }
}
