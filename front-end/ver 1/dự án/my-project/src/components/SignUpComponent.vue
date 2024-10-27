<template>
  <div class="divider"></div>
  <div class="signupform">
    <div class="form-container">
      <h2 class="text-center text-xl font-bold mb-4">Create Your Account</h2>
      <form @submit.prevent="onSubmit">
        <input type="text" v-model="username" placeholder="Username" required />
        <input type="text" v-model="fullname" placeholder="Fullname" required />
        <input type="email" v-model="email" placeholder="Email" required />
        <input
          type="password"
          v-model="password"
          placeholder="Password"
          required
        />

        <div class="flex items-center agree-container">
          <div>
            <input type="checkbox" v-model="agree" id="agree" required />
          </div>
          <label class="text-sm ml-1" for="agree"
            >You agree to create an account</label
          >
        </div>

        <button type="submit" class="sign-up">SIGN UP</button>
        <button type="button" class="cancel" @click="onCancel">CANCEL</button>
        <p class="text-center text-sm">
          Already signed up? <router-link to="/login">Login</router-link>
        </p>
      </form>
    </div>

    <div class="illustration">
      <img
        :src="imageSrc"
        alt="Illustration of people reading books and studying"
        width="400"
        height="400"
      />
    </div>
  </div>
</template>

<script>
export default {
  name: "SignUpComponent",
  data() {
    return {
      imageSrc: require("@/assets/123456.png"),
      username: "",
      fullname: "",
      email: "",
      password: "",
      agree: false,
    };
  },
  methods: {
    onSubmit() {
      // Logic xử lý đăng ký
      console.log(
        "Form submitted:",
        this.username,
        this.fullname,
        this.email,
        this.password,
        this.agree
      );
    },
    onCancel() {
      // Xử lý khi nhấn nút Cancel
      this.username = "";
      this.fullname = "";
      this.email = "";
      this.password = "";
      this.agree = false;
      this.$router.push('/login');
    },
  },
};
</script>

<style scoped>
body {
  background-color: #1a2238;
  font-family: "Arial", sans-serif;
}

.divider {
  width: 10%;
  height: 2px;
  background-color: #fbbf24;
  position: absolute;
  top: 55px;
  left: 50%;
  transform: translateX(-50%);
  border-radius: 5px;
}

.signupform {
  display: flex;
  justify-content: center;
  align-items: center;
  height: 100vh;
  background-color: #1a2238;
  padding-top: 3%;
}

.form-container {
  background-color: #e0e3eb;
  border-radius: 20px;
  padding: 2rem;
  width: 30%;
  height: 89%;
  margin-right: 4%;
}

.form-container input {
  background-color: #ffffff;
  border: 1px solid #d1d5db;
  border-radius: 5px;
  padding: 0.5rem;
  margin-bottom: 1rem;
  width: 100%;
}

.form-container button {
  border-radius: 5px;
  padding: 0.5rem;
  width: 100%;
  margin-bottom: 1rem;
}

.form-container .sign-up {
  background-color: #fbbf24;
  color: #ffffff;
}

.form-container .cancel {
  background-color: #ffffff;
  color: #6b7280;
  border: 1px solid #d1d5db;
}

.form-container a {
  color: #3b82f6;
}

.illustration {
  margin-left: 50px;
  display: flex;
  align-items: center;
}

img {
  max-width: 100%;
  height: auto;
}

.agree-container {
  display: flex;
  align-items: center;
}
</style>
