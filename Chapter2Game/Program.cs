namespace Chapter2Game
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Player player = new Player();
            while (true)
            {
                char answer = ShowMainScreen();//메인스크린 호출 및 값 받아오기

                if (answer == '1')
                {
                    ShowStatus(player);//상태보기 수행
                }
                else if (answer == '2')
                {
                    Console.WriteLine("인벤토리 실행");
                    //인벤토리 
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
            public String chad = "전사";
            public int attacklevel = 10;
            public int defencelevel = 5;
            public int hp = 100;
            public int gold = 1500;
        }
    }
}
