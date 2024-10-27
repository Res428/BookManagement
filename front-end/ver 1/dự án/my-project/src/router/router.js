import { createRouter, createWebHistory } from "vue-router";
import LoginComponent from "../components/LoginComponent.vue";
import SignUpComponent from "@/components/SignUpComponent.vue";
import HomeComponent from "@/components/HomeComponent.vue";
import AccountComponent from "@/components/AccountComponent.vue";
import SchedulingComponent from "@/components/SchedulingComponent.vue";
import MyOrderComponent from "@/components/MyOrderComponent.vue";
import NotificationComponent from "@/components/NotificationComponent.vue";
import WaitApprovalComponent from "@/components/WaitApprovalComponent.vue";
import BookDetailComponent from "@/components/BookDetailComponent.vue";

// Định nghĩa các routes
const routes = [
  {
    path: "/home",
    name: "HomeComponent",
    component: HomeComponent,
    meta: {
      requiresAuth: true,
    },
  },
  {
    path: "/login",
    name: "LoginComponent",
    component: LoginComponent,
  },
  {
    path: "/sign-up",
    name: "SignUpComponent",
    component: SignUpComponent,
  },
  {
    path: "/account",
    name: "AccountSetting",
    component: AccountComponent,
    meta: {
      requiresAuth: true,
    },
  },
  {
    path: "/rent",
    name: "SchedulingComponent",
    component: SchedulingComponent,
    meta: {
      requiresAuth: true,
    },
  },
  {
    path: "/orders",
    name: "MyOrderComponent",
    component: MyOrderComponent,
    meta: {
      requiresAuth: true,
    },
  },
  {
    path: "/notifications",
    name: "NotificationComponent",
    component: NotificationComponent,
    meta: {
      requiresAuth: true,
    },
  },
  {
    path: "/wait-approval",
    name: "WaitApprovalComponent",
    component: WaitApprovalComponent,
    meta: {
      requiresAuth: true,
    },
  },
  {
    path: "/books/:id",
    component: BookDetailComponent,
  },
  {
    path: "/:pathMatch(.*)*",
    redirect: "/login",
  },
];

// Tạo router
const router = createRouter({
  history: createWebHistory(),
  routes,
});

// Middleware kiểm tra đăng nhập
router.beforeEach((to, from, next) => {
  const isAuthenticated = localStorage.getItem("auth"); // Hoặc kiểm tra token từ store

  if (to.meta.requiresAuth && !isAuthenticated) {
    // Nếu cần đăng nhập mà chưa đăng nhập, điều hướng về trang login
    next("/login");
  } else {
    // Nếu đã đăng nhập hoặc không cần bảo mật, cho phép điều hướng
    next();
  }
});

export default router;
