#!/bin/bash
dm=`date +%d%b`
mysqldump -u xcanalyze xca_database > backups/xca_database$dm.mysql
