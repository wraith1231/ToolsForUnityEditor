# ToolsForUnityEditor
유니티로 작업하다가 귀찮아서 만들어둔 스크립트 모음용입니다. 나중에 돌려쓸 수 있어서 보관용.    

Player State Maker  
플레이어용 State를 간단하게 만들 수 있게 만들었습니다. 기본적으로 만들어진 state 파일은 Assets/Scripts/Player/Super 폴더 혹은 Sub 폴더에 들어가게 됩니다. HFSM 형태로 작업하느라 이렇게 만들었습니다. 다른 루트를 원하시면 PlayerSuperPath와 PlayerSubPath 값을 변경해주시면 됩니다.  
템플릿도 그냥 텅 빈 생성자(플레이어 상태 기계를 인자로 받는), Enter, Exit, Update를 가지고 있는 클래스입니다. 인자에 값을 주면 생성자 _Value_ 부분이 그걸로 바뀌어 들어갑니다.  
해당 에디터는 유니티 에디터 상단 Tools/Make Player Super State Maker...는 수정하겠습니다. 아무튼 거기서 생성하면 위 폴더에 P + 상태 이름 + State.cs 파일이 생성됩니다. 이 부분도 OnGUI 함수의 GUILayout.Button 부분들을 변경해주시면 됩니다.  
상속받고있는 IState는 만들어주셔야합니다만, 기본으로 플레이어 상태 기계를 인자로 받는걸 보시면 아시겠지만 interface가 아니라 abstract 클래스입니다. Enter, Exit, Update는 abstract로 생성해두시고 생성자만 플레이어 상태 기계를 인자로 받어서 저장하게 해두시면 됩니다. 필요없으시면 삭제해주시고 state maker의 템플릿 부분도 생성자에 인자를 안받게 수정해주시면 됩니다.  
이후 필요하면 수정 예정  
25-08-13 약간 수정, 지금 State 변경을 어떻게 처리할지 고민중이라 그에 따라 더 변할 수 있습니다.  
25-08-19 State Maker에서 Player State Maker로 변경, Enemy 용은 따로 만들기로 했습니다. Input Manager에서 입력을 받아올 예정으로 그에 맞춰서 여러가지 수정과 super, sub 생성에 유의미한 차이를 뒀습니다. sub는 생성자에 super도 필수가 되었습니다. 노가다가 좀 있었네요.  
    
Attack Data Editor  
공격 데이터를 담은 Json 파일을 에디터에서 수정할 수 있게 만들었습니다. AttackData 클래스가 json의 각 요소들입니다. Container는 그냥 wrapper. EnumExtensions에 IsDefined 정의한 것도 제가 처음에 enum 말고 string으로 하다가 enum으로 바꾼 놈들 때문에 썼습니다.  
json 파일은 Assets/Data/AttackData.json 입니다. 없으면 자동으로 생성되게 만들었습니다. 다른 위치에 두고싶다면 DataPath 값을 변경해주시면 됩니다.  
AttackData에 값을 추가하거나 수정하게 된다면 주석으로도 써있지만 LoadJsonData, DrawAttackData 두 함수도 변경해주셔야합니다. 자동으로 하려면 SerializedObject로 해야되는데 그럼 이것저것 꼬이게 되가지고 귀차니즘을 감수하기로 했습니다. 필요하시면 attackdata, container 클래스 둘다 serializedObject 상속시키고 전체적으로 수정해주시면 됩니다.  
변경한 값이 자동으로 저장되는게 아니라 상단에 save all 버튼을 클릭해주셔야 저장이 됩니다. 저장 안하고 끄면 데이터 다 날라갑니다.  
이후 필요하면 또 수정 예정  
25-08-25 데이터 List에 Scroll을 추가했습니다.

       
Animtation Editor  
애니메이션 fbx 파일을 불러들인 후 그 안에 들어있는 rig의 애니메이션 타입(휴머노이드 같은거), 애니메이션에서 발 떨림 현상 줄이기(anti jitter), 전체 애니메이션 클립의 look root rotation, lock root position xz, loop time 값 변경을 일괄적으로 수정할 수 있도록 만들었습니다. 파일은 asset 폴더 안에 있는 파일만 불러들일 수 있습니다. 아니면 오류먹더라구요.  
직접 애니메이션 만들어서 하는거 아니면 크게 쓸모없는 에디터일 수 도...  

Enemy Node Maker
아직 만드는 중, 이번엔 player state maker랑 다르게 수정 기능도 넣으려니까 엄청 복잡해지네요...
