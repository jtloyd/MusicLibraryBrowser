#!/usr/bin/python
import sys, psycopg2
from configparser import ConfigParser

class pgConn:
    _conn = None

    def config(self, filename='database.ini', section='postgresql'):
        # create a parser
        parser = ConfigParser()
        # read config file
        parser.read(filename)
     
        # get section, default to postgresql
        db = {}
        if parser.has_section(section):
            params = parser.items(section)
            for param in params:
                db[param[0]] = param[1]
        else:
            raise Exception('Section {0} not found in the {1} file'.format(section, filename))
     
        return db
     
    def connect(self):
        """ Connect to the PostgreSQL database server """

        try:
            # read connection parameters
            params = self.config()
     
            # connect to the PostgreSQL server
            print('Connecting to the PostgreSQL database...')
            self._conn = psycopg2.connect(**params)
     
            # create a cursor
            cur = self._conn.cursor()
            
     # execute a statement
            print('PostgreSQL database version:')
            cur.execute('SELECT version()')
     
            # display the PostgreSQL database server version
            db_version = cur.fetchone()
            print(db_version)
           
         # close the communication with the PostgreSQL
            cur.close()
        except (Exception, psycopg2.DatabaseError) as error:
            print(error)
            
    def close(self):
        if self._conn is not None:
            self._conn.close()
            print('Database connection closed.')

    def execute(self, sql, *vartuple):
        idv = None
        try:
            cur = self._conn.cursor()
            cur.execute(sql, vartuple)
            idv = cur.fetchone()[0]
            self._conn.commit()
            cur.close()
        except (Exception, psycopg2.DatabaseError) as error:
            print(error)
        return idv
        
 
