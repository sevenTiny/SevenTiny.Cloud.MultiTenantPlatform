import React, { Component } from 'react';
import IceContainer from '@icedesign/container';
import './IndexView.scss';
import TableList from '../TableList';
import SearchForm from '../SearchForm';
import dataApiHost from '../../commonConfig';
import axios from 'axios';
import { Table } from '@alifd/next';

//请求视图页组件的接口
const indexComponentApi = dataApiHost + '/api/UI/IndexPage';

const styles = {
  container: {
    padding: '0 0 20px',
  },
  title: {
    margin: '0',
    padding: '15px 20px',
    fonSize: '16px',
    textOverflow: 'ellipsis',
    overflow: 'hidden',
    whiteSpace: 'nowrap',
    color: 'rgba(0,0,0,.85)',
    fontWeight: '500',
    borderBottom: '1px solid #eee',
  },
  pagination: {
    textAlign: 'right',
  },
};

export default class IndexView extends Component {
  constructor(props) {
    super(props);
    this.state = {
      title: '请假记录',
      searchDatas: null,
      loadTable: false
    }
    this.getData()
  }

  // MOCK 数据，实际业务按需进行替换
  getData = () => {
    var th = this;
    //请求数据
    axios({
      method: 'get',
      url: indexComponentApi + '?viewName=' + this.props.ViewName,
    }).then(function (response) {
      if (response.status == 200) {
        if (response.data.success) {
          var result = response.data.data
          console.log(result)
          th.setState({
            loadTable: true
          })
          // bizDatas = result.biz_data.map((item, index) => {
          //   var obj = {}
          //   for (var columnItem in result.sub_cmps) {
          //     //列编码
          //     var columnName = result.sub_cmps[columnItem].cmp_data.name;
          //     //添加该列的字段
          //     obj[columnName] = item[columnName].text;
          //   }
          //   //宽度赋值
          //   return obj;
          // });
        }
      } else {
        console.error(response)
      }
    });
  };

  //条件改变重新赋值
  conditionChange = (value) => {
    //创建搜索条件数组
    var searchDatas = [];
    for (var item in value) {
      if (value[item] != null && value[item] != '') {
        searchDatas.push({
          Name: item,
          Text: null,
          Value: value[item]
        });
      }
    }
    this.setState({
      searchDatas: searchDatas
    })
  };

  render() {
    return (
      <IceContainer style={styles.container}>
        <h4 style={styles.title}>{this.state.title}</h4>
        <SearchForm onChange={this.conditionChange} />
        {
          this.state.loadTable ? <TableList searchDatas={this.state.searchDatas} ViewName={this.props.ViewName} MetaObject={this.props.MetaObject} /> : ''
        }
      </IceContainer>
    );
  }
}

