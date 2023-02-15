###Github 사용법

***
###내가 Project Host일때 (내가 총대를 메고 repository를 만들어서 팀원들에게 배포해야 할때)

####1.깃허브 사이트에서 repository를 만든다.

####2.프로젝트 폴더와 repository를 동기화 한다.
- git init
- git add README.md
- git commit -m “메시지”
  - 메시지는 아래 커밋 메시지 작성 규칙대로 작성합니다. 여기서는 첫 repository 연동이므로 "first commit" 이라고 입력하셔도 됩니다.
- git branch -M main
- git remote add origin 주소
- git push origin main
  - 아이디와 비번을 요구하는 경우 입력한다
    - 토큰 발행(없다면)
      비번을 요구할때 깃허브 로그인 비밀번호가 아닌, token을 입력해야 합니다.

***
###Guest일 경우(Host가 배포한 repo를 사용하는 경우)

####PC에 프로젝트 폴더 생성
- git clone “url주소”
  - url주소는 repository주소를 입력하면 됩니다. 이 repository의 경우, https://github.com/seraict23/ParkPro.git 입니다.

***
###공통
####자기 브랜치 만들기
- git branch 브랜치명
  - 예시: git branch feature/Park01/dimLineMaker


###프로젝트 업로드

- git add .
- git commit -M “메시지”
- git push origin feature/Park-002/dimLineMaker(브랜치명)
  - 설명)
  이메일로 파일을 보낸다고 생각해보면 쉽습니다. 우리가 이메일을 보낼때, (1)누구에게 보낼지 주소를 입력해야하고, (2)무슨 파일을 보낼지 파일을 선택해야 합니다 (3)그리고 메시지를 작성합니다  
  깃허브도 마찬가지지만 순서가 (2) (3) (1)로 다를 뿐입니다. 
  먼저 add . 은 해당 폴더 내의 모든 파일을 가리킵니다. 즉 모든 파일을 업로드 한다는 의미입니다.
  commit -M "메시지"에 내용을 입력하되 메시지 입력 규칙은 하단을 참조하세요.
  push origin 브랜치명 에서 origin은 url주소를 담고 있는 변수입니다. 호스트가 첨에 repository를 배포할때, "git remote add origin 주소"를 입력했던 것을 기억하십시오. 이것은 origin이라는 변수를 선언하여 거기에 url주소를 담은 것입니다. 브랜치명은 보통 feature/아이디-이슈번호/description 형태로 쓰는데, 자세한 브랜치 명명 규칙은 하단을 참조하세요.

프로젝트 업로드는 매일 작업을 마치고 습관처럼 하도록 합시다.


### 작업하던 프로젝트 다운로드

- git pull
  - 업로드 방식에는 git fetch도 있고 숙련자가 되면 이것을 더 많이 사용하게 될 것이지만, 일단은 pull만 알아둡시다.

매일 작업을 마치고 프로젝트 업로드를 습관처럼 하듯이, 매일 아침 컴퓨터를 켜고 이것부터 시작하는 습관을 들이도록 합시다.


###기타 알아두면 좋은 명령어들
- git status: 어디까지 커밋이 돼있는지 알아볼 수 있습니다


***
###브랜치 관리와 merge rule

이상의 방법들은 깃허브의 repo를 만들어서 업로드하고 다운로드하는 방법까지만을 설명한 것입니다. 이제부터는 협업과 커뮤니케이션을 위한 깃허브 권장 사용 규칙에 대해 알아봅시다.
먼저 브랜치와 머지-룰 입니다.

깃허브에는 브랜치라는 개념이 있습니다. 
브랜치는 크게 3종류가 사용됩니다.
**Main**
**Dev**
**Feature**

우리가 코드를 작업하면 바로 Main에 올리는 것이 아니라, 먼저 feature에 올려서 작업을 하고, 검사를 거쳐서 dev로 승격되고, 최종 승인을 거쳐서 Main에 올라가게 됩니다.

항상 코드는 오류 가능성을 내포하고 있습니다. 버그로 오염된 코드를 회사 전체 프로젝트에 올려서 프로젝트가 오작동하는 경우를 막기 위하여, 개인의 실험적인 코딩은 feature에서만 작업을 하고 가급적 dev와 main에는 오류없는 코드를 올려놓기 위함입니다.

* Main은 판매를 위한 완결된 코드만을 올려둡니다. 고객이 요구할 경우 언제라도 시연하고 판매할 수 있는 완결된 코드 입니다.
* Dev는 판매 바로 전단계. 어느정도의 완결성은 갖추고 있지만, 테스트나 상급자의 결재 등의 절차를 기다리고 있는 상태의 코드입니다.
* Feature에는 개인이 작업중인 완성이든 완성되지 않았든 관계없이, 자유롭게 코드를 올립니다.

즉 작업자들은 작업한 코드를 feature 브랜치에 자유롭게 올립니다.
그러다가 어느정도 완성 되었다. 이정도면 팀원들이 만든 코드에 붙여도 민폐가 되지 않겠다 판단되면, dev브랜치에 merge합니다.
dev브랜치에 merge된 코드들이 모여서 테스트, 상급자의 결정 등의 절차를 거쳐 main에 merge되고 판매를 개시합니다.


따라서 각 회사들은 merge rule을 정해야 합니다. 통상적인 merge rule은 다음과 같습니다.
####Merge Rule
#####feature에서 dev로 승격
- 일반적인 환경에서 정상적으로 작동
- 그 작동 여부를 본인 외 1인이 확인

#####dev에서 main으로 승격
- 다양한 환경 있을 수 있는 거의 모든 시나리오에서 작동하는 테스트 통과
- 프로젝트 최종 권한을 가진 사람의 승인


####Branch 명명 규칙
브랜치의 이름은 다음과 같이 정합니다
#####feature/작업한분아이디-이슈번호/기능에 대한 간단한 description
예) feature/Park-002/DimLineMaker


####merge 하는 방법
만약 feature/Park-002/DimLineMaker 브랜치를 dev로 머지 시킨다고 가정하면,
> git checkout dev
> git merge feature/Park-002/DimLineMaker


###커밋 메시지 

깃허브는 팀원들과의 협업과 커뮤니케이션을 위한 도구입니다. 설령 feature브랜치에 개인의 실험적인 코드를 올릴때도 커밋 메시지를 아무렇게나 해서는 안됩니다

커밋 메시지의 형식은 다음과 같습니다
####커밋메시지 형식
> <Type>: <Title>
> <Body>
> <Footer>
바디와 푸터는 선택사항입니다. 타이틀로 설명이 충분하다면 안써도 됩니다


####타입의 종류
타입의 종류는 다음과 같습니다.
- feat : 새로운 기능 추가
- fix : 버그 수정
- docs : 문서 수정
- style : 코드 formatting, 세미콜론(;) 누락, 코드 변경이 없는 경우
- refactor : 코드 리팩터링
- test : 테스트 코드, 리팩터링 테스트 코드 추가(프로덕션 코드 변경 X)
- chore : 빌드 업무 수정, 패키지 매니저 수정(프로덕션 코드 변경 X)
- design : CSS 등 사용자 UI 디자인 변경
- comment : 필요한 주석 추가 및 변경
- rename : 파일 혹은 폴더명을 수정하거나 옮기는 작업만인 경우
- remove : 파일을 삭제하는 작업만 수행한 경우
- !BREAKING CHANGE : 커다란 API 변경의 경우
- !HOTFIX : 급하게 치명적인 버그를 고쳐야 하는 경


####7대 규칙
> 1. 제목과 본문을 한 줄 띄어 구분
> 2. 제목은 50자 이내
> 3. 제목 첫 글자는 대문자
> 4. 제목 끝에 마침표 X
> 5. 제목은 명령문으로, 과거형 X
> 6. 본문의 각 행은 72자 이내 (줄바꿈 사용)
> 7. 본문은 어떻게 보다 무엇을, 왜에 대하여 설명

회의를 통해 외국인 이용자들이 우리 제품을 사용하는데 어려움을 겪고 있으니, 영어메뉴를 추가하자는 결론이 나왔고, 이 안건에 143번 번호를 붙인 상태. 
해당 기능을 추가하였다면 커밋메시지를 다음과 같이 작성하면 됩니다.

> git commit -m “feat: Add menus for English user

> Added Korean/English switch icon to the top navigation bar.

> Click this icon to display all menus in English.

> resolves: #143”


Q. 커맨드라인에서 엔터키 입력해도 되나요? 중간에 그냥 명령어 입력돼버리는거 아닌가요?

커맨드라인에서 따옴표”를 닫기 전까지는 엔터키 입력해도 괜찮습니다
