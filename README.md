# csdt2223ki46bozhyk02
Subject: CSDT

#### Task 4. Implement data driven approach and DB (FEF):

1. SW(client) should store input and output messages, process time and other data
in MySQL DB.
2. Create SQL script to create DB.
3. Create a config file with data driven format from table#1 to store all parameters
required for SW(client): (UART, HW i-face parameters, DB communication
string…)
4. Required steps.

#### Task 3. Create pipelines/actions for SW(client) (FEF):

1. Create YML file with next features:
build all binaries (create scripts in folder ci/ if need);
run tests;
create artifacts with binaries and test reports;
2. Commit changes to feature/develop/<task number>;
3. Create pull request with name <task number> and submit author as reviewer;
4. After the reviewer approved – merge request into develop branch.

#### Task 2 - Create UART<->i-face schema (FEF):

1. Create a simple schema SW(client) <-> UART <-> HW(bridge) <-> HW i-fase
<-> HW(server).
NOTE: that SW(client) is NOT a terminal or other downloaded SW. It is SW
developed by students.
2. The client should send a message through the bridge to the server. The server
should modify the message and send it back to the client through the bridge.
3. Required steps

#### Task 1 - Initiate GIT repository (BEF):

1. Create a github repo with the name “csdt<student'sname><student's number>”, main branch develop. YY1 – Start studding year YY2 – End studding year For example: 2019-2020 -> 1920 Full example csdt1920ki47spitzeras03
2. Add access for the author.
3. Create a branch feature/develop/. For example feature/develop/task1.
4. Create README file with: a. details about repo; b. task details; c. student number.
5. Create GIT TAG: __WW ∙ YY – current year; ∙ WW – work week; ∙ D – current day number of weak.
6. Create pull request with name task1 and submit author as reviewer.
7. After the reviewer approved – merge request into develop branch.
 
#### task details:
+ Student: Bozhyk Oleh Maryanovych
+ Students number: 2 
+ HW i-face: I2C
+ Game: Sea battle game 
+ Data driven format: JSON
