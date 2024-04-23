using System.Security.Cryptography.X509Certificates;

namespace Chapter2Game
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //초기설정
            Player player = new Player();
            //MakeItems();

            List<Item> invenList = new List<Item>();

            Item mu_shuePlate = new Item("이름", "타입", 999, "착용함", true, "여기에이야기");
            invenList.Add(mu_shuePlate);

            while (true)
            {
                char answer = ShowMainScreen();//메인스크린 호출 및 값 받아오기

                if (answer == '1')
                {
                    ShowStatus(player);//상태보기 수행
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

        //public static void MakeItems()
        //{
        //    Item mu_shuePlate;
        //    mu_shuePlate.name = "무쇠갑옷";
        //    mu_shuePlate.type = "방어력";
        //    mu_shuePlate.value = 5;
        //    mu_shuePlate.doesEquip = "E";
        //    mu_shuePlate.story = "무쇠로 만들어져 튼튼한 갑옷입니다";

        //    Item spartaSpear;
        //    spartaSpear.name = "스파르타의 창";
        //    spartaSpear.type = "공격력";
        //    spartaSpear.value = 7;
        //    spartaSpear.doesEquip = "";
        //    spartaSpear.story = "스파르타의 전사들이 사용했다는 전설의 창입니다.";

        //    Item oldSword;
        //    mu_shuePlate.name = "낡은 검";
        //    mu_shuePlate.type = "공격력";
        //    mu_shuePlate.value = 2;
        //    mu_shuePlate.doesEquip = "";
        //    mu_shuePlate.story = "쉽게 볼 수 있는 낡은 검 입니다.";
        //}

        private static void ShowStatus(Player player)//상태보기 수행
        {
            Console.Clear();
            Console.WriteLine("상태 보기\n캐릭터의 정보가 표시됩니다\n");
            Console.WriteLine("Lv : {0}", player.level);
            Console.WriteLine("Chad : {0}", player.chad);
            Console.WriteLine("공격력 : {0}", player.attacklevel);
            Console.WriteLine("방어력 : {0}", player.defencelevel);
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
            Console.Clear();
            Console.WriteLine("인벤토리\n보유 중인 아이템을 관리할 수 있습니다.\n");
            Console.WriteLine("[아이템 목록]\n");

            foreach(Item exitem in invenList)
            {
                Console.WriteLine(" - {0}{1}\t| 방어력 +{2} | {3}", exitem.doesEquip,exitem.name,exitem.value,exitem.story);

            }

            Console.WriteLine("1. 장착 관리\n");
            Console.WriteLine("0. 나가기\n");
            Console.WriteLine("원하시는 행동을 입력해주세요");

            while (char.Parse(Console.ReadLine()) != '0')
            {
                Console.WriteLine("올바른 값을 입력하세요");
            }
            Console.Clear();

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

        //class Inventory//item타입의 list로 변경
        //{

        //}

        //struct Item
        //{
        //    public string name;
        //    public string type;
        //    public int value;
        //    public string doesEquip;
        //    public bool doesHave;
        //    public string story;
        //}
        class Item
        {
            public string name;
            public string type;
            public int value;
            public string doesEquip;
            public bool doesHave;
            public string story;


            public Item(string name_, string type_, int value_, string doesEquip_, bool doesHave_, string story_)
            {
                name = name_;
                type = type_;
                value = value_;
                doesEquip = doesEquip_;
                doesHave = doesHave_;
                story = story_;

            }

        }

    }
}
