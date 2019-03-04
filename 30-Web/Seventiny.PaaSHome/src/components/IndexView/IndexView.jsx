import React, { Component } from 'react';
import IceContainer from '@icedesign/container';
import './IndexView.scss';
import TableList from '../TableList';
import SearchForm from '../SearchForm';
import dataApiHost from '../../commonConfig';
import axios from 'axios';

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
    this.getData()
  }
  
  state = {
    icon: null,
    title: '',
    searchDatas: null,
    layoutType: -1,//布局类型
    loadComponents: false,//是否加载组件标识
    searchItems: []
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
          th.setState({
            icon: result.icon,
            title: result.title,
            layoutType: result.layout_type,
            loadComponents: true,
            searchItems: result.search_form.search_items
          })
          // console.log('log1')//log
          // console.log(th.state)//log
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

  getTitle = () => {
    return <h4 style={styles.title}>{this.state.title}</h4>
  }

  getSearchForm = () => {
    if (this.state.searchItems != null && this.state.searchItems.length > 0) {
      return <SearchForm onChange={this.conditionChange} searchItems={this.state.searchItems} />
    }
  }

  getTableList = () => {
    return <TableList searchDatas={this.state.searchDatas} ViewName={this.props.ViewName} MetaObject={this.props.MetaObject} />
  }

  render() {
    return (
      <IceContainer style={styles.container}>
        {this.getTitle()}
        {this.getSearchForm()}
        {this.getTableList()}
      </IceContainer>
    );
  }
}

