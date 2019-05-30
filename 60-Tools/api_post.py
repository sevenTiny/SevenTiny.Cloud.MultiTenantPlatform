import requests
import datetime

url = 'http://localhost:39011/api/CloudData?_interface=WangDongApp.InterfaceTest.Interface.BatchAdd'

i = 18

data = {
    'age': i,
    'Name': '张三_'+str(i),
    'Weight': 65.5,
    'TrueOrFalse': True
}
response = requests.post(url, json=data)
print(response.text)
