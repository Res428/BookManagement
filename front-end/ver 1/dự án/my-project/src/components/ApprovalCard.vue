<template>
    <div class="approval-card">
      <img :src="imageSrc" alt="Book Cover" class="book-cover" />
      <div class="info">
        <p>{{ description }}</p>
        <button @click="approve" class="approve-button">Approve</button>
        <button @click="reject" class="reject-button">Reject</button>
      </div>
    </div>
  </template>
  
  <script>
  export default {
    name: 'ApprovalCard',
    props: {
      imageSrc: {
        type: String,
        required: true
      },
      description: {
        type: String,
        required: true
      }
    },
    methods: {
      approve() {
        // Logic to approve the request
        alert('Approved!');
      },
      reject() {
        // Logic to reject the request
        alert('Rejected!');
      }
    }
  }
  </script>
  
  <style scoped>
  .approval-card {
    background-color: #f9c2c2;
    border-radius: 5px;
    padding: 10px;
    margin: 10px;
    width: 200px;
  }
  
  .book-cover {
    width: 100%;
    height: auto;
  }
  
  .info {
    display: flex;
    flex-direction: column;
  }
  
  .approve-button,
  .reject-button {
    margin-top: 5px;
  }
  </style>
  