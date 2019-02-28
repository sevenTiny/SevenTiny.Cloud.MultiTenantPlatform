import React, { Component } from 'react';
import IceContainer from '@icedesign/container';
import './IndexView.scss';
import TableList from '../TableList';
import SearchForm from '../SearchForm';

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
      searchDatas: null
    }
  }

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
        <h4 style={styles.title}>请假记录</h4>
        <SearchForm onChange={this.conditionChange} />
        <TableList searchDatas={this.state.searchDatas} ViewName={this.props.ViewName} MetaObject={this.props.MetaObject} />
      </IceContainer>
    );
  }
}

