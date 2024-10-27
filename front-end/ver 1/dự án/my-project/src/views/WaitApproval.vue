<template>
    <div class="wait-approval">
      <h2>Wait Approval</h2>
      <div class="date-selector">
        <span>Mon, 9 Sep 2024</span>
        <span>Sat, 14 Sep 2024</span>
      </div>
      <div class="approval-cards">
        <ApprovalCard
          v-for="(item, index) in items"
          :key="index"
          :imageSrc="item.image"
          :description="item.description"
        />
      </div>
    </div>
  </template>
  
  <script>
  import ApprovalCard from '../components/ApprovalCard.vue';
  
  export default {
    name: 'WaitApproval',
    components: {
      ApprovalCard
    },
    data() {
      return {
        items: [
          {
            image: 'https://via.placeholder.com/150',
            description: 'Lorem ipsum Lorem ipsum Lorem ipsum'
          },
          // Thêm các yêu cầu khác nếu cần
        ]
      };
    }
  }
  </script>
  
  <style scoped>
  .wait-approval {
    padding: 20px;
  }
  
  .date-selector {
    display: flex;
    justify-content: space-between;
    margin-bottom: 20px;
  }
  
  .approval-cards {
    display: flex;
    flex-wrap: wrap;
  }
  </style>
  