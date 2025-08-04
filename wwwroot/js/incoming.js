$(document).ready(function () {
    const rowsPerPage = 6;
    let currentPage = 1;
    let totalRecords = 0;
    const tableBody = $('#incomingTable tbody');
    const pageInfo = $('#pageInfo');
    const prevPageBtn = $('#prevPage');
    const nextPageBtn = $('#nextPage');

    async function fetchIncomingData() {
        tableBody.html('<tr><td colspan="7" style="text-align:center;"><i class="fas fa-spinner fa-spin"></i> กำลังโหลดข้อมูล...</td></tr>');

        try {
            const response = await $.ajax({
                url: '/Incoming/GetPaginatedIncomingData',
                method: 'GET',
                dataType: 'json',
                data: {
                    pageNumber: currentPage,
                    pageSize: rowsPerPage
                }
            });

            const incomingData = response.data;
            totalRecords = response.totalRecords;

            renderTable(incomingData);
            updatePaginationControls();

        } catch (error) {
            console.error("เกิดข้อผิดพลาดในการดึงข้อมูล Incoming:", error);
            tableBody.html('<tr><td colspan="7" style="text-align:center;">เกิดข้อผิดพลาดในการโหลดข้อมูล กรุณาลองใหม่อีกครั้ง</td></tr>');
        }
    }

    function renderTable(dataToRender) {
        tableBody.empty();

        if (dataToRender.length === 0) {
            tableBody.append('<tr><td colspan="7" style="text-align:center;">ไม่พบข้อมูล</td></tr>');
            return;
        }

        const rows = dataToRender.map(item => {
            const statusInfo = getStatusDisplayInfo(item.status);
            return `
                <tr>
                    <td>${item.productId}</td>
                    <td>${item.productName || '-'}</td>
                    <td>${item.category || '-'}</td>
                    <td>${item.supplierName}</td>
                    <td>${item.expectedQuantity}</td>
                    <td>${item.expectedArrivalDate || '-'}</td>
                    <td>
                        <span class="status-cell ${statusInfo.className}">
                            <span class="status-text">${item.status}</span>
                        </span>
                    </td>
                </tr>
            `;
        });
        tableBody.append(rows.join(''));
    }

    function getStatusDisplayInfo(status) {
        let className = '';
        switch (status.toLowerCase()) {
            case 'pending':
                className = 'status-pending';
                break;
            case 'arrived':
                className = 'status-arrived';
                break;
            case 'delayed':
                className = 'status-delayed';
                break;
            case 'cancelled':
                className = 'status-cancelled';
                break;
            default:
                className = 'status-default';
                break;
        }
        return {
            className
        };
    }

    function updatePaginationControls() {
        const totalPages = Math.ceil(totalRecords / rowsPerPage);
        pageInfo.text(`หน้า ${currentPage} จาก ${totalPages}`);
        prevPageBtn.prop('disabled', currentPage === 1);
        nextPageBtn.prop('disabled', currentPage === totalPages || totalPages === 0);
    }

    prevPageBtn.on('click', function () {
        if (currentPage > 1) {
            currentPage--;
            fetchIncomingData();
        }
    });

    nextPageBtn.on('click', function () {
        const totalPages = Math.ceil(totalRecords / rowsPerPage);
        if (currentPage < totalPages) {
            currentPage++;
            fetchIncomingData();
        }
    });

    fetchIncomingData();
});