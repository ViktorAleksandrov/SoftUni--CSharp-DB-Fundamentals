BACKUP DATABASE SoftUni TO DISK='D:\softuni-backup.bak'
DROP DATABASE SoftUni
RESTORE DATABASE SoftUni FROM DISK='D:\softuni-backup.bak'