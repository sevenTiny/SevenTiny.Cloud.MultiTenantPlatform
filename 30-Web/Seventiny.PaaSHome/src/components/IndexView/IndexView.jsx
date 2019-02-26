import React, { Component } from 'react';
import IceContainer from '@icedesign/container';
import './IndexView.scss';

const styles = {
  exceptionContent: {
    display: 'flex',
    justifyContent: 'center',
    alignItems: 'center',
  },
  title: {
    color: '#333',
  },
  description: {
    color: '#666',
  },
};

export default class IndexView extends Component {
  static displayName = 'IndexView';
  render() {
    return (
      <div className="basic-not-found">
        <IceContainer>
          <div style={styles.exceptionContent} className="exception-content">
            <div className="prompt">
              <h3 style={styles.title} className="title">
                抱歉，你访问的页面不存在
              </h3>
              <p style={styles.description} className="description">
                您要找的页面没有找到，请返回
                <a href="#">首页</a>
                继续浏览
              </p>
            </div>
          </div>
        </IceContainer>
      </div>
    );
  }
}

