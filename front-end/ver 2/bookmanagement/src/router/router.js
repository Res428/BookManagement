import { createRouter, createWebHistory } from "vue-router";
import LoginComponent from "../components/LoginComponent.vue";
import SignUpComponent from "@/components/SignUpComponent.vue";
import HomeComponent from "@/components/HomeComponent.vue";

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
