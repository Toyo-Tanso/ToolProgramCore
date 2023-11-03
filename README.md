# Tools Program

<img src="${Images}\tools.png" alt="ruler Icon" style="zoom:10%" />


### General description

This app replaced a VB 2003 program that tracked the tool calibers for the machines. It is a semi-complex ASP.NET Core program that provides basic CRUD operations to maintain ISO certification requirements.  It includes various functions to its users such as:

* Check in a tool
  * Records username and deviation from standard dimension
* Temporarily borrow tool
  * Tracks if the tool is borrowed to another workstation and records when it is expected back
  * Records return of tool
* Exports checking in data to users
* Has Admin page where admin can
  * Add/Remove Work Centers
  * Add/Remove Employees
  * Add/Remove/Edit Tool list
  * Add/Remove/ Verified users

This program is intended to work on an in-network IIS deployment. This App is **not** yet adjusted to be deployed outside of the network. There several features that allow users to streamline entering information such as searchable dropdowns and auto-filling Work Centers. The database exists on the internal SQLSERVER, that maintains some of the logic. 



### Developer Note:

Please take into consideration the design of the underlying database when making adjustments. There are some views that help explain the relationships of the tables. The ID's of the tables correlate to other tables so they are all interconnected. Please be wary of changing schema or moving to another database to avoid any error breaking in the code.



### Issues

* Fix Navigation on add Measure page
* View All on checked out tools breaks when list is empty





### Technical Debt Table

|      |      |      |
| ---- | ---- | ---- |
|      |      |      |
|      |      |      |
|      |      |      |



### Photos



#### Home Page

<img src="${Images}\image-20231103104721857.png" alt="image-20231103104721857" style= "zoom:80%;" />

### Recent tool Measure list

<img src="${Images}\image-20231103105852815-1699036120642-5.png" style="zoom:80%" />



#### Add Measure

<img src="${Images}\image-20231103104858517.png" alt="image-20231103104858517" style="zoom:80%;" />

### Checked out tools

<img src="${Images}\image-20231103110233199.png" alt="image-20231103110233199" style="zoom:80%;" />

### Check out tool

<img src="${Images}\image-20231103110303702.png" alt="image-20231103110303702" style="zoom:80%;" />

### Admin console login

<img src="${Images}\image-20231103110405991.png" alt="image-20231103110405991" style="zoom:80%;" />

### WorkCenters Page

<img src="${Images}\image-20231103110725202.png" alt="image-20231103110725202" style="zoom: 67%;" />

### Employees

<img src="${Images}\image-20231103110916005.png" alt="image-20231103110916005" style="zoom:80%;" />

### Tools

<img src="${Images}\image-20231103111009500.png" alt="image-20231103111009500" style="zoom:80%;" />

### Verified Users

<img src="${Images}\image-20231103111057024.png" alt="image-20231103111057024" style="zoom:80%;" />
