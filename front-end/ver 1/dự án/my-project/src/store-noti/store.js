// store.js
import { createStore } from 'vuex';

const store = createStore({
  state: {
    notifications: [],
  },
  mutations: {
    ADD_NOTIFICATION(state, message) {
      state.notifications.push({ message, read: false }); // Thêm thuộc tính 'read'
    },
    CLEAR_NOTIFICATIONS(state) {
      state.notifications = [];
    },
    MARK_ALL_AS_READ(state) {
      state.notifications.forEach(notification => {
        notification.read = true; // Đánh dấu tất cả là đã đọc
      });
    },
  },
  actions: {
    addNotification({ commit }, message) {
      commit('ADD_NOTIFICATION', message);
    },
    clearNotifications({ commit }) {
      commit('CLEAR_NOTIFICATIONS');
    },
    markAllAsRead({ commit }) {
      commit('MARK_ALL_AS_READ');
    },
  },
  getters: {
    unreadNotifications(state) {
      return state.notifications.filter(notification => !notification.read); // Lấy danh sách thông báo chưa đọc
    },
  },
});

export default store;
