import React, { Component } from 'react';
import IndexView from '../../components/IndexView';

export default class PaasHome extends Component {
  render() {
    return (
      <div>
        <IndexView ViewName={"WangDongApp.DataTest.IndexView.TestIndexView"} MetaObject={"WangDongApp.DataTest"}/>
      </div>
    );
  }
}
