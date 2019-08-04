import requests
import datetime

url = 'http://localhost:39011/api/BatchCloudData?_interface=WangDongApp.InterfaceTest.Interface.BatchAdd'

datas = []
for i in range(1,100000):
    data = {
        # 'positiveNumber': i,
        # 'Text': 'Text测试'+str(i),
        # 'DateTime': str(datetime.datetime.now()),
        # 'TrueOrFalse': True,
        # 'Integer': i,
        # 'Long': i,
        # 'Double': i
        'age': i,
        'Name': '张三_'+str(i),
        'Weight': 65.5,
        'TrueOrFalse': True
    }
    datas.append(data)

response = requests.post(url, json=datas)
print(response.text)
