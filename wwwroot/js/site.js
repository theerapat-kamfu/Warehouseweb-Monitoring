document.addEventListener('DOMContentLoaded', () => {
            const bubbleContainer = document.querySelector('.bubble-container');
            const numBubbles = 40; 

            function createBubble() {
                const bubble = document.createElement('div');
                bubble.classList.add('bubble');

                const size = Math.random() * 80 + 30; 
                bubble.style.width = `${size}px`;
                bubble.style.height = `${size}px`;

                const startX = Math.random() * 100; 
                bubble.style.left = `${startX}vw`;

             
                const translateXAmount = (Math.random() - 0.5) * 200; 
                const scaleAmount = Math.random() * 0.8 + 0.5;

                bubble.style.setProperty('--translateX', `${translateXAmount}px`);
                bubble.style.setProperty('--scale', scaleAmount);

                const animationDuration = Math.random() * 10 + 10; 
                const animationDelay = Math.random() * numBubbles * (animationDuration / numBubbles) * -1; // ทำให้เริ่มแอนิเมชันไม่พร้อมกัน

                bubble.style.animationDuration = `${animationDuration}s`;
                bubble.style.animationDelay = `${animationDelay}s`;

                bubbleContainer.appendChild(bubble);

               
                bubble.addEventListener('animationend', () => {
                    bubble.remove();
                    createBubble();
                });
            }

           
            for (let i = 0; i < numBubbles; i++) {
                createBubble();
            }
});